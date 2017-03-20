using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories.Cards;
using MediatR;
using Models.Models.API;

namespace API.Features.Card
{
    public class GetCards
    {
        public class Request : IAsyncRequest<Result> { }
        public class Result : ApiResult<IEnumerable<Models.Models.Cards.Card>> { }
        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ICardRepository _cardRepository;

            public Handler(ICardRepository cardRepository)
            {
                _cardRepository = cardRepository;
            }

            public async Task<Result> Handle(Request message)
            {
                var cards = await _cardRepository.GetAllCards();
                var result = new Result { Data = cards };

                return result;
            }
        }
    }
}
