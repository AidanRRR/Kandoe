using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories.Cards;
using MediatR;
using Models.Models.API;

namespace API.Features.Card
{
    public class GetCard
    {
        public class Request : IAsyncRequest<Result>
        {
            public string CardId { get; set; }
        }
        public class Result : ApiResult<Models.Models.Cards.Card> { }
        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ICardRepository _cardRepository;

            public Handler(ICardRepository cardRepository)
            {
                _cardRepository = cardRepository;
            }

            public async Task<Result> Handle(Request message)
            {
                try
                {
                    var card = await _cardRepository.GetCard(message.CardId);
                    var result = new Result { Data = card };

                    return result;
                }
                catch
                {
                    return new Result
                    {
                        HasErrors = true,
                        ErrorMessages = new List<string> { $"Error while retrieving Card : {message.CardId}" }
                    };
                }
            }
        }
    }
}
