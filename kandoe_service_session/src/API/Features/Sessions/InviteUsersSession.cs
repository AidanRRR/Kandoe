using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Mail;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using MongoDB.Driver;
using BL;
using System.Linq;
using Models.Models.Session;

namespace API.Features.Sessions
{
    public class InviteUsersSession
    {
        public class Request : IAsyncRequest<Result> {
            public Guid SessionId { get; set; }
            public string Email { get; set; } 
            public string Token { get; set; }
        }

        public class Result : ApiResult<bool>
        { }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            private readonly IMailSender _mailSender;
            private readonly IServiceManager _serviceManager;

            public Handler(ISessionEventsRepository sessionEventsRepository, IMailSender mailSender, IServiceManager serviceManager)
            {
                _sessionEventsRepository = sessionEventsRepository;
                _mailSender = mailSender;
                _serviceManager = serviceManager;
            }

            public async Task<Result> Handle(Request message)
            {
                var result = new Result();
                var session = await _sessionEventsRepository.GetSession(message.SessionId);
                
                result.Data = false;

                // Check of sessie geldig
                if (session == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Sessie bestaat niet");
                    return result;
                }

                // Check of al uitgenodigd
                if (session.InvitedUserEmails.Contains(message.Email))
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Gebruiker is al uitgenodigd voor deze sessie");
                    return result;
                }

                // Kan enkel uitnodigen voor sessies die nog niet begonnen zijn
                if (session.Phase != SessionPhase.Planned)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Kan enkel mensen uitnodigen voor een geplande sessie.");
                    return result;
                }

                // Check JWT token
                var isMgr = await _serviceManager.IsTokenManager(message.SessionId, message.Token);
                if (!isMgr)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Enkel thema beheerders mogen mensen uitnodigen voor een sessie");
                    return result;
                } 

                // Toevoegen aan database
                await _sessionEventsRepository.InviteUsersToSession(session.SessionId, message.Email);
                // Email verzenden
                try
                {
                    await _mailSender.SendEmailAsync(message.Email, message.SessionId.ToString(), session);
                }
                catch (Exception ex) 
                {
                    // Whatever
                }

                result.Data = true;
                return result;
            }
        }
    }
}
