using System;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Session;
using BL;
using Microsoft.AspNetCore.SignalR;
using Models.Models.Events;

namespace API.Features.Sessions
{
    public class StartSession
    {
        public class Request : IAsyncRequest<Result>
        {
            public string Token {get; set; }
            public Guid SessionId { get; set; }
        }

        public class Result : ApiResult<bool>
        { }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            private readonly ISessionManager sessionManager;
            private readonly IHubContext<SessionHub> hubContext;
            private readonly IServiceManager serviceManager;

            public Handler(ISessionEventsRepository sessionEventsRepository, ISessionManager sessionManager, IHubContext<SessionHub> hubContext, IServiceManager serviceManager)
            {
                _sessionEventsRepository = sessionEventsRepository;
                this.sessionManager = sessionManager;
                this.hubContext = hubContext;
                this.serviceManager = serviceManager;
            }

            public async Task<Result> Handle(Request message)
            {
                var result = new Result();
                var session = await _sessionEventsRepository.GetSession(message.SessionId);

                if (session == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Sessie bestaat niet");
                } 
                else
                {
                    string err;
                    if (!sessionManager.IsSessionStartable(session.SessionId, out err))
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add(err);
                        return result;
                    }

                    var isMgr = await serviceManager.IsTokenManager(message.SessionId, message.Token);
                    if (!isMgr) {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("U bent geen manager van deze sessie.");
                        return result;
                    }

                    if (session.Phase != SessionPhase.Planned)
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Deze sessie is al actief of afgelopen.");
                    }
                    else
                    {
                        var ok = sessionManager.LoadSession(session.SessionId);
                        if (ok)
                        {
                            result.Data = true;
                            sessionManager.UpdateSessionPhase(session.SessionId, SessionPhase.Active);
                            var state = sessionManager.GetSessionState(session.SessionId);
                            state.Session.Phase = SessionPhase.Active;
                            var sse = new SessionStartEvent()
                            {
                                SessionId = session.SessionId,
                                Session = state.Session,
                                Players = sessionManager.GetSessionUsers(state.Session)
                            };
                            sessionManager.ProcessEvent(state, sse);
                            hubContext.Clients.Group(session.SessionId.ToString()).onSessionEvent(sse);
                            sessionManager.PersistEvent(sse);
                        }
                        else
                        {
                            result.HasErrors = true;
                            result.ErrorMessages.Add("Laden van sessie mislukt.");
                        }
                    }
                }

                return result;
            }
        }
    }
}