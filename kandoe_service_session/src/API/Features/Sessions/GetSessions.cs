using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Events.Dto;


namespace API.Features.Sessions
{
    public class GetSessions
    {
        public class Request : IAsyncRequest<Result> { }

        public class Result : ApiResult<IEnumerable<SessionDto>>
        {}

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;

            public Handler(ISessionEventsRepository sessionEventsRepository)
            {
                _sessionEventsRepository = sessionEventsRepository;
            }

            public async Task<Result> Handle(Request message)
            {
                var sessionEvents = await _sessionEventsRepository.GetAllSessions();
                var result = new Result { Data = sessionEvents };

                return result;
            }
        }
    }
}
