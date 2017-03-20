using System;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories.Cards;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;
using Models.Models.Cards;

namespace API.Features.Card
{
    public class AddReaction
    {
        public class Request : IAsyncRequest<Result>
        {
            public string CardId { get; set; }
            public string Username { get; set; }
            public string Message { get; set; }
        }

        public class Result : ApiResult<Models.Models.Cards.Card>{}

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

            public async Task<Result> Handle(Request msg)
            {
                try
                {
                    var reaction = new Reaction(){ Username = msg.Username, CardId = msg.CardId, Message = msg.Message};
                    var card = await _cardRepository.AddReaction(reaction);
                    var theme = await _themeRepository.ReplaceCardOfTheme(card.ThemeId, msg.CardId, card);
                    return new Result(){Data = card};
                }
                catch (Exception e)
                {
                     var result = new Result {HasErrors = true};
                    result.ErrorMessages.Add(e.StackTrace);
                    return result;
                }
            }
        }
    }
}