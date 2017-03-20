using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Models.Models.Events;
using Models.Models.Session;
using Microsoft.AspNetCore.SignalR.Hubs;
using BL;
using Microsoft.Extensions.Logging;

namespace API
{
    [HubName("SessionHub")]
    public class SessionHub : Hub
    {
        private ISessionManager sessionManager;
        private ILogger logger;


        // Hebben de tickers niet echt nodig hier willen gewoon dat de singleton(s)
        // geintialiseerd worden met HubContext...
        public SessionHub(ITurnTicker turnTicker, ISessionTicker sessionTicker, ISessionManager sessionManager, ILogger<SessionHub> logger)
        {
            this.sessionManager = sessionManager;
            this.logger = logger;
            this.logger.LogDebug("Nieuwe SessionHub aangemaakt...");
        }

        private void Error(Guid sessionId, string type, string message)
        {
            Clients.Caller.onSessionEvent(new ErrorEvent()
            {
                Message = message,
                ErrorType = type,
                SessionId = sessionId
            });
        }

        private void FireEvent(ISessionEvent e, bool callerOnly, bool processLocally = true, bool persist = true)
        {
            if (processLocally)
            {
                var state = sessionManager.GetSessionState(e.SessionId);
                if (state == null)
                {
                    throw new ArgumentException("Session niet gevonden " + e.SessionId.ToString());
                }
                sessionManager.ProcessEvent(state, e);
            }

            if (callerOnly)
            {
                Clients.Caller.onSessionEvent(e);
            }
            else
            {
                Clients.Group(e.SessionId.ToString()).onSessionEvent(e);
            }

            if (persist)
            {
                sessionManager.PersistEvent(e);
            }
        }

        public void SendMessage(Guid sessionId, string message)
        {
            if (Context.IsInSession(sessionId, false))
            {
                var state = sessionManager.GetSessionState(sessionId);    

                var e = new ChatMessageEvent()
                {
                    SessionId = sessionId,
                    Message = message,
                    UserId = Context.UserId(),
                    TurnNr = state.TurnNr
                };

                FireEvent(e, false);
            }
            else
            {
                logger.LogDebug("Chat message genegeerd: gebruiker niet in connected users...");
            }
        }

        public void MoveCard(Guid sessionId, string cardId)
        {
            logger.LogDebug("Move card: " + sessionId + " -- " + cardId);
            if (Context.IsInSession(sessionId, false))
            {
                if (sessionManager.CanMove(sessionId, Context.UserId(), cardId))
                {
                    var state = sessionManager.GetSessionState(sessionId);    

                    // Broadcast move naar alle clients  in session
                    var me = new MoveEvent()
                    {
                        SessionId = sessionId,
                        UserId = Context.UserId(),
                        TurnNr = state.TurnNr,
                        CardId = cardId
                    };
                    FireEvent(me, false);

                    // Volgende beurt starten
                    var tse = new TurnStartEvent()
                    {
                        SessionId = sessionId, 
                        UserId = state.Players[(state.TurnNr + 1) % state.Players.Count].UserName,
                        TurnNr = state.TurnNr + 1
                    };
                    FireEvent(tse, false);

                    // Volledige ronde gespeeld
                    if (((state.TurnNr - 1) % state.Players.Count) == 0)
                    {
                        sessionManager.AddSnapshot(sessionId, "", false);
                    }
                }
                else
                {
                    logger.LogDebug("Mag deze kaart niet verplaatsen...");
                }
            }
            else
            {
                logger.LogDebug("Chat message genegeerd: gebruiker niet in connected users...");
                // TODO: foutafhandeling
            }
        }

        public void EndSession(Guid sessionId)
        {
            if (!sessionManager.IsManager(sessionId, Context.UserId()))
            {
                Error(sessionId, "NOT_ALLOWED", "Enkel beheerders kunnen eens sessie beeindigen");
                return;
            }

            var ok = sessionManager.UpdateSessionPhase(sessionId, SessionPhase.Finished);
            if (!ok)
            {
                logger.LogInformation("Updaten session phase mislukt... " + sessionId.ToString());
                return;
            }

            var state = sessionManager.GetSessionState(sessionId);
            if (state == null)
            {
                logger.LogInformation("Sessie niet gevonden... " + sessionId.ToString());
                return;
            }

            var e = new SessionEndEvent()
            {
                SessionId = sessionId
            };
            FireEvent(e, false);
            sessionManager.UpdateSessionPhase(sessionId, SessionPhase.Finished);
        }

        public void TakeSnapshot(Guid sessionId)
        {
            logger.LogInformation(Context.UserId());
            if (!sessionManager.IsManager(sessionId, Context.UserId()))
            {
                logger.LogInformation("Gebruiker mag geen snapshot nemen " + sessionId.ToString());
                Error(sessionId, "NOT_ALLOWED", "Enkel beheerders kunnen snapshots aanmaken.");
                return;
            }
            logger.LogInformation("Gebruiker mag snapshot nemen " + sessionId.ToString());
            
            var ok = sessionManager.AddSnapshot(sessionId, "", true);
            if (!ok)
            {
                logger.LogInformation("Aanmaken snapshot mislukt");
            }
            else
            {
                logger.LogDebug("Snapshot aangemaakt");
            }
        }

        public void PickCards(Guid sessionId, string[] cardIds)
        {
            if (Context.IsInSession(sessionId, false))
            {
                if (sessionManager.CanPick(sessionId, Context.UserId(), cardIds))
                {
                    var state = sessionManager.GetSessionState(sessionId);
                    var cpe = new CardPickEvent()
                    {
                        SessionId = sessionId,
                        Cards = cardIds,
                        TurnNr = state.TurnNr,
                        Timestamp = DateTime.Now,
                        UserId = Context.UserId(),
                    };
                    FireEvent(cpe, false);

                    if (sessionManager.CheckCardPickingFinished(sessionId))
                    {
                        logger.LogInformation("Iedereen heeft kaarten gekozen, start spel " + sessionId.ToString());
                        var picks = sessionManager.MergePicks(sessionId);
                        var gse = new GameStartEvent()
                        {
                            SessionId = sessionId,
                            Timestamp = DateTime.Now,
                            Cards = picks.ToArray()
                        };
                        Console.WriteLine(picks.ToString());

                        FireEvent(gse, false);
                    }
                }
                else {
                    logger.LogDebug("Gebruiker " + Context.UserId() + " mag geen kaarten kiezen " + sessionId.ToString());
                }
            }
            else
            {
                logger.LogDebug("Gebruiker " + Context.UserId() + " niet in sessie " + sessionId.ToString());
            }
        }
            
        public void Connect(string token, Guid sessionId)
        {
            var ssn = sessionManager.GetSessionState(sessionId);
            if (ssn == null)
            {
                Error(sessionId, "CONNECT", "Deze sessie is niet actief.");
                return;
            }

            var userId = sessionManager.RegisterClient(sessionId, token);
            if (userId != null) 
            {
                Context.SetUserId(userId);
                // Huidige state sturen naar nieuw verbonden gebruiker
                var ce = new ConnectEvent()
                {
                    SessionId = sessionId,
                    State = sessionManager.GetSessionState(sessionId)
                };
                FireEvent(ce, true, false, false);

                // Voeg toe aan session groep 
                Groups.Add(Context.ConnectionId, sessionId.ToString());
            }
            else
            {
                Error(sessionId, "CONNECT", "U hebt geen toegang tot deze sessie.");
            }
        }
    }

}
