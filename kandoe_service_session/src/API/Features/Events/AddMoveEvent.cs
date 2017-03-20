using System;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Events;
using Models.Models.Events.Dto;

namespace API.Features.Events
{
    public class AddMoveEvent
    {
        public class Request : IAsyncRequest<AddMoveEvent.Result>
        {
            public Guid SessionId { get; set; }
            public MoveEvent MoveEvent;
        }

        public class Result : ApiResult<Models.Models.Events.ISessionEvent> { }

        public class Handler : IAsyncRequestHandler<AddMoveEvent.Request, AddMoveEvent.Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            public Handler(ISessionEventsRepository sessionEventsRepository)
            {
                _sessionEventsRepository = sessionEventsRepository;
            }

            public async Task<AddMoveEvent.Result> Handle(AddMoveEvent.Request message)
            {
                try
                {
                    var newMoveEvent = new Models.Models.Events.MoveEvent()
                    {
                        SessionId = message.MoveEvent.SessionId,
                        CardId = message.MoveEvent.CardId,
                        Timestamp = DateTime.UtcNow,
                        TurnNr = message.MoveEvent.TurnNr,
                        Type = message.MoveEvent.Type,
                        UserId = message.MoveEvent.UserId
                    };

                    await _sessionEventsRepository.AddSessionEvent(new SessionEventDto()
                    {
                        SessionId = newMoveEvent.SessionId, 
                        SessionEvent = newMoveEvent
                    });

                    var result = new Result { Data = newMoveEvent };
                    return result;
                }
                catch
                {
                    var result = new Result { HasErrors = true };
                    return result;
                }
            }
        }
    }
}
