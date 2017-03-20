using System;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Events;
using Models.Models.Events.Dto;
using MongoDB.Driver;

namespace API.Features.Events
{
    public class AddChatMessageEvent
    {
        public class Request : IAsyncRequest<AddChatMessageEvent.Result>
        {
            public Guid SessionId { get; set; }
            public ChatMessageEvent ChatMessageEvent;
        }

        public class Result : ApiResult<UpdateResult> { }

        public class Handler : IAsyncRequestHandler<AddChatMessageEvent.Request, AddChatMessageEvent.Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            public Handler(ISessionEventsRepository sessionEventsRepository)
            {
                _sessionEventsRepository = sessionEventsRepository;
            }

            public async Task<AddChatMessageEvent.Result> Handle(AddChatMessageEvent.Request message)
            {
                try
                {
                    var sessionEventDto = new SessionEventDto()
                    {
                        SessionId = message.SessionId,
                        SessionEvent = message.ChatMessageEvent
                    };
                    var response = await _sessionEventsRepository.AddSessionEvent(sessionEventDto);

                    var result = new Result { Data = response };
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
