using BL;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using Models.Models.Events;
using Models.Models.Session;
using Microsoft.Extensions.Logging;

namespace API
{
    public class SessionTicker : ISessionTicker
    {
        // Via DI
        // https://github.com/aspnet/SignalR/issues/182
        private IHubContext<SessionHub> hubContext;
        private Timer scheduledSessionTimer;
        private Timer endSessionTimer;
        private ISessionManager sessionManager;
        private ILogger logger;

        public SessionTicker(IHubContext<SessionHub> hubContext, ISessionManager sessionManager, ILogger<SessionTicker> logger)
        {
            this.hubContext = hubContext;
            this.sessionManager = sessionManager;
            this.logger = logger;
            scheduledSessionTimer = new Timer(ScheduledSessionTick, null, 1000, 60000);
            endSessionTimer = new Timer(EndSessionTick, null, 1000, 60000);
            RestoreSessions();
        }

        public void ScheduledSessionTick(object arg)
        {
            var sessions = sessionManager.GetStartingSessions();
            foreach (var session in sessions)
            {
                string err;
                if (!sessionManager.IsSessionStartable(session, out err))
                {
                    logger.LogInformation("Kan sessie niet starten: " + err);
                    continue;
                }

                logger.LogInformation("Starten van geschedulde sessie " + session.SessionId.ToString());
                
                sessionManager.LoadSession(session.SessionId);
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
        }

        public void EndSessionTick(object arg)
        {
            // TODO: check actieve die beeindigd moeten worden...
        }

        public void RestoreSessions()
        {
            var sessions = sessionManager.GetActiveSessions();
            foreach (var session in sessions)
            {
                logger.LogInformation("Herstellen van sessie " + session.SessionId.ToString());
                sessionManager.LoadSession(session.SessionId);
            }
        }
    }
}