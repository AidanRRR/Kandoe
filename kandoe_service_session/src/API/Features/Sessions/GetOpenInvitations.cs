
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Session;
using Models.Models.Events.Dto;
using BL;
using System.Linq;

namespace API.Features.Sessions
{
    public class GetOpenInvitations
    {
        public class Request : IAsyncRequest<Result>
        {
            public string Token { get; set; }
        }

        public class Result : ApiResult<IEnumerable<SessionDto>>
        { }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ISessionEventsRepository sessionEventsRepository;
            private readonly IServiceManager serviceManager;

            public Handler(ISessionEventsRepository sessionEventsRepository, IServiceManager serviceManager)
            {
                this.sessionEventsRepository = sessionEventsRepository;
                this.serviceManager = serviceManager;
            }

            public async Task<Result> Handle(Request message)
            {
                var result = new Result();

                var userName = await serviceManager.VerifyToken(message.Token);
                if (userName == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Geen geldig JWT token");
                }
                else
                {
                    var user = await serviceManager.GetUser(userName);
                    if (user == null)
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Ophalen user data mislukt");
                    }
                    else
                    {
                        var sessions = await sessionEventsRepository.GetInvitedSessions(user.Email);
                        result.Data = sessions.Where(s => s.Phase == SessionPhase.Planned).ToList();
                    }
                }
                return result;
            }
        }
    }
}