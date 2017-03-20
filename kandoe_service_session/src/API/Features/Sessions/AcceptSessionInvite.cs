using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using MongoDB.Driver;
using BL;
using Models.Models.Session;

namespace API.Features.Sessions
{
    public class AcceptSessionInvite
    {
        public class Request : IAsyncRequest<Result>
        {
            public Guid SessionId { get; set; }
            public string Token { get; set; }
        }

        public class Result : ApiResult<bool>
        {
            
        }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            private readonly IServiceManager _serviceManager;

            public Handler(ISessionEventsRepository sessionEventsRepository, IServiceManager serviceManager)
            {
                _sessionEventsRepository = sessionEventsRepository;
                _serviceManager = serviceManager;
            }

            public async Task<Result> Handle(Request message)
            {
                var result = new Result();
                result.Data = false;

                var session = await _sessionEventsRepository.GetSession(message.SessionId);

                if (session == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Sessie bestaat niet.");
                    return result;
                }

                // Check of het niet te laat is om te accepteren.
                if (session.Phase != SessionPhase.Planned)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Kan enkel voor geplande sessies.");
                    return result;
                }

                // Token verifieren
                var userName = await _serviceManager.VerifyToken(message.Token);
                if (userName == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Ongeldig token.");
                    return result;
                }

                // User ophalen
                var user = await _serviceManager.GetUser(userName);
                if (user == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("User data ophalen mislukt.");
                    return result;
                } 

                // Check of uitgenodigd is.
                if (!session.InvitedUserEmails.Contains(user.Email))
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("U bent niet uitgenodigd voor deze sessie.");
                    return result;
                }

                await _sessionEventsRepository.AcceptSessionInvite(session.SessionId, userName, user.Email);

                result.Data = true; 
                return result;
            }
        }
    }
}
