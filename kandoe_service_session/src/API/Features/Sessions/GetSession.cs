using System;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Events.Dto;

namespace API.Features.Sessions
{
    public class GetSession
    {
        public class Request : IAsyncRequest<GetSession.Result>
        {
            public Guid SessionId { get; set; }
        }

        public class Result : ApiResult<SessionDto>
        { }

        public class Handler : IAsyncRequestHandler<GetSession.Request, GetSession.Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;

            public Handler(ISessionEventsRepository sessionEventsRepository)
            {
                _sessionEventsRepository = sessionEventsRepository;
            }

            public async Task<GetSession.Result> Handle(GetSession.Request message)
            {
                var sessionEvents = await _sessionEventsRepository.GetSession(message.SessionId);
                var result = new GetSession.Result { Data = sessionEvents };

                return result;
            }
        }
    }
}