using BL;
using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using Models.Models.Events;
using Microsoft.Extensions.Logging;

namespace API
{
    // Deze klasse controleert periodiek of er beurten van actieve sessies verlopen zijn
    // Als dit het geval is wordt de volgende beurt gestart en deze informatie gebroadcast
    // naar alle verbonden clients voor die sessie
    public class TurnTicker: ITurnTicker
    {
        // Via DI
        // https://github.com/aspnet/SignalR/issues/182
        private IHubContext<SessionHub> hubContext;
        private Timer timer;
        private ISessionManager sessionManager;
        private ILogger logger;

        public TurnTicker(IHubContext<SessionHub> hubContext, ISessionManager sessionManager, ILogger<TurnTicker> logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            this.sessionManager = sessionManager;
            timer = new Timer(Tick, null, 1000, 1000);
        }

        public void Tick(object arg = null)
        {
            var expiredTurnSessions = sessionManager.GetExpiredTurnSessions();
            foreach (var sid in expiredTurnSessions)
            {
                var sessionId = Guid.Parse(sid);
                logger.LogDebug("Automatisch starten van volgende beurt voor sessie " + sid);
                var state = sessionManager.GetSessionState(sessionId);

                // Volgende beurt starten
                //state.TurnNr++;
                var tse = new TurnStartEvent
                {
                    SessionId = sessionId, 
                    UserId = state.Players[(state.TurnNr + 1) % state.Players.Count].UserName,
                    TurnNr = state.TurnNr + 1
                };
                sessionManager.ProcessEvent(state, tse);
                this.hubContext.Clients.Group(sid).onSessionEvent(tse);
                sessionManager.PersistEvent(tse);

                // Volledige ronde gespeeld
                if (((state.TurnNr - 1) % state.Players.Count) == 0)
                {
                    sessionManager.AddSnapshot(sessionId, "", false);
                }
            }
        }
    }
}