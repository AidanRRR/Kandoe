using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories.Cards;
using MediatR;
using Models.Models.API;
using Models.Models.Cards;

namespace API.Features.Card
{
    public class UpdateCard
    {
        public class Request : IAsyncRequest<Result>
        {
            public string CardId { get; set; }
            public string ImageUrl { get; set; }
            public string Text { get; set; }
            public string ThemeId { get; set; }
            public bool IsEnabled { get; set; } = true;
            public List<Reaction> Reactions { get; set; }
        }
        public class Result : ApiResult<Models.Models.Cards.Card> { }
        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ICardRepository _cardRepository;
            private readonly IMapper _mapper;

            public Handler(ICardRepository cardRepository, IMapper mapper)
            {
                _cardRepository = cardRepository;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Request message)
            {
                var result = new Result();

                if (message.CardId == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"Card with ID {message.CardId} was not found");
                    return result;
                }

                try
                {
                    
                    var updatedCard = _mapper.Map<Models.Models.Cards.Card>(message);
                    updatedCard.UpdatedOn = DateTime.UtcNow;
                    
                    result.Data = updatedCard;

                    await _cardRepository.UpdateCard(updatedCard);
                }
                catch (Exception)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"An error has occured while attempting to update Card: {message.CardId}");
                }

                return result;
            }
        }
    }
}
