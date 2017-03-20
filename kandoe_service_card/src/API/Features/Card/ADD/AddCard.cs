using System;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories.Cards;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;

namespace API.Features.Card
{
    public class AddCard
    {
        public class Request : IAsyncRequest<Result>
        {
            public string ImageUrl { get; set; }
            public string Text { get; set; }
            public string ThemeId { get; set; }
            public bool IsEnabled { get; set; } = true;
        }

        public class Result : ApiResult<Models.Models.Cards.Card> { }

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
                    var newCard = _mapper.Map<Models.Models.Cards.Card>(message);

                    await _cardRepository.AddCard(newCard);

                    var theme = _themeRepository.GetTheme(message.ThemeId).Result;
                    theme.Cards.Add(newCard);

                    await _themeRepository.UpdateCardsOfTheme(theme);

                    var result = new Result { Data = newCard };
                    return result;

                }
                catch(Exception e)
                {
                    var result = new Result {HasErrors = true};
                    result.ErrorMessages.Add(e.StackTrace);
                    return result;
                }
            }
        }
    }
}