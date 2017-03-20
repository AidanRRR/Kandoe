using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Events;
using Models.Models.Events.Dto;

namespace API.Features.Events
{
    public class AddConnectEvent
    {
        public class Request : IAsyncRequest<AddConnectEvent.Result>
        {
            public Guid SessionId { get; set; }
            public ConnectEvent ConnectEvent;
        }

        public class Result : ApiResult<Models.Models.Events.ISessionEvent> { }

        public class Handler : IAsyncRequestHandler<AddConnectEvent.Request, AddConnectEvent.Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            public Handler(ISessionEventsRepository sessionEventsRepository)
            {
                _sessionEventsRepository = sessionEventsRepository;
            }

            public async Task<AddConnectEvent.Result> Handle(AddConnectEvent.Request message)
            {
                try
                {
                    var connectEvent = new Models.Models.Events.ConnectEvent()
                    {
                        SessionId = message.SessionId,
                        Timestamp = DateTime.UtcNow,
                        State = message.ConnectEvent.State,
                        TurnNr = message.ConnectEvent.TurnNr,
                        Type = message.ConnectEvent.Type,
                        UserId = message.ConnectEvent.UserId
                    };

                    await _sessionEventsRepository.AddSessionEvent(new SessionEventDto()
                    {
                        SessionId = connectEvent.SessionId,
                        SessionEvent = connectEvent
                    });
                    
                    var result = new Result { Data = connectEvent };

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
