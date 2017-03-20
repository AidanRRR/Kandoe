using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories.Cards;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;

namespace API.Features.Card
{
    public class AddCards
    {
        public class Request : IAsyncRequest<Result>
        {
            public List<Models.Models.Cards.Card> Cards { get; set; }
        }

        public class Result : ApiResult<List<Models.Models.Cards.Card>> { }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ICardRepository _cardRepository;
            private readonly IThemeRepository _themeRepository;
            private readonly IMapper _mapper;

            public Handler(ICardRepository cardRepository, IThemeRepository themeRepository, IMapper mapper)
            {
                _cardRepository = cardRepository;
                _themeRepository = themeRepository;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Request message)
            {
                try
                {
                    var cards = message.Cards.AsEnumerable();
                    await _cardRepository.AddCards(cards);
                    var theme = await _themeRepository.GetTheme(cards.FirstOrDefault().ThemeId);

                    if (theme != null)
                    {
                        foreach (var c in cards)
                        {
                            theme.Cards.Add(c);
                        }
                    }

                    await _themeRepository.UpdateCardsOfTheme(theme);
                    return new Result { Data = message.Cards };
                }
                catch (Exception e)
                {
                    var result = new Result { HasErrors = true };
                    result.ErrorMessages.Add(e.StackTrace);
                    return result;
                }
            }
        }
    }
}
