using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Events.Dto;
using System;

namespace API.Features.Sessions
{
    public class GetThemeSessions
    {
        public class Request : IAsyncRequest<Result> 
        { 
            public Guid ThemeId { get; set; }
        }

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
                var result = new Result();
            
                if (message.ThemeId.Equals(Guid.Empty))
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Ongeldige thema ID.");
                }
                else
                {
                    var sessions = await _sessionEventsRepository.GetThemeSessions(message.ThemeId);
                    result.Data = sessions;
                }
                return result;
            }
        }
    }
}
