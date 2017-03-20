using System;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Events;
using Models.Models.Events.Dto;

namespace API.Features.Events
{
    public class AddTurnStartEvent
    {
        public class Request : IAsyncRequest<AddTurnStartEvent.Result>
        {
            public Guid SessionId { get; set; }
            public TurnStartEvent TurnStartEvent;
        }

        public class Result : ApiResult<Models.Models.Events.ISessionEvent> { }

        public class Handler : IAsyncRequestHandler<AddTurnStartEvent.Request, AddTurnStartEvent.Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            public Handler(ISessionEventsRepository sessionEventsRepository)
            {
                _sessionEventsRepository = sessionEventsRepository;
            }

            public async Task<AddTurnStartEvent.Result> Handle(AddTurnStartEvent.Request message)
            {
                try
                {
                    var newTurnStartEvent = new Models.Models.Events.TurnStartEvent()
                    {
                        SessionId = message.SessionId,
                        Timestamp = DateTime.UtcNow,
                        TurnNr = message.TurnStartEvent.TurnNr,
                        Type = message.TurnStartEvent.Type,
                        UserId = message.TurnStartEvent.UserId
                    };

                    await _sessionEventsRepository.AddSessionEvent(new SessionEventDto()
                    {
                        SessionId = newTurnStartEvent.SessionId,
                        SessionEvent = newTurnStartEvent
                    });

                    var result = new Result { Data = newTurnStartEvent };
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
