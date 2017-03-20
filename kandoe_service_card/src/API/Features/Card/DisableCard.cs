using System;
using System.Threading.Tasks;
using DAL.Repositories.Cards;
using MediatR;
using Models.Models.API;

namespace API.Features.Card
{
    public class DisableCard
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
                var result = new Result();

                var card = await _cardRepository.GetCard(message.CardId);

                if (card == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"Card with ID {message.CardId} was not found");
                    return result;
                }

                try
                {
                    card.IsEnabled = false;
                    result.Data = card;

                    await _cardRepository.DisableCard(message.CardId);
                }
                catch (Exception)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"An error has occured while attempting to disable Card: {message.CardId}");
                }

                return result;
            }
        }

    }
}
