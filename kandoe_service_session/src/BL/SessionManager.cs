using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Models;
using Models.Models.Chat;
using Models.Models.Events;
using Models.Models.Session;
using Models.Models.Imported;
using Models.Models.API;
using System.Net.Http;
using Newtonsoft.Json;
using DAL.Repositories;
using Models.Models.Events.Dto;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BL
{
    public class SessionManager: ISessionManager
    {
        // Map van geladen sessions
        private ConcurrentDictionary<Guid, SessionState> ActiveSessions = new ConcurrentDictionary<Guid, SessionState>();

        private ISessionEventsRepository repository;
        private IServiceManager services;
        private ILogger logger;

        public SessionManager(ISessionEventsRepository repo, IServiceManager services, ILogger<SessionManager> logger)
        {
            this.repository = repo;
            this.services = services;
            this.logger = logger;

            /*var cards = new Card[]
            {
                new Card()
                {
                    ImageUrl = "assets/bier.jpg",
                    Text = "Bier",
                    ThemeId = "a"
                },
                new Card()
                {
                    ImageUrl = "assets/koffie.jpg",
                    Text = "Koffie",
                    ThemeId = "a"
                },
                new Card()
                {
                    ImageUrl = "assets/thee.jpg",
                    Text = "Thee",
                    ThemeId = "a"
                },
                new Card()
                {
                    ImageUrl = "assets/Water.jpg",
                    Text = "Water",
                    ThemeId = "a"
                },
                new Card()
                {
                    ImageUrl = "assets/Wijn.jpg",
                    Text = "Wijn",
                    ThemeId = "a"
                },
            };
            var players = new string[]
            {
                "Maarten",
                "admin",
            };
            var themeId = Guid.Parse("29949969-6156-403d-9e55-ff3e98d93b6c");

            var s1 = new Session()
            {
                Cards = cards,
                PlayerIds = players,
                ThemeId = themeId,
                TurnTime = 60,
                PickTime= 180,
                Phase = SessionPhase.Planned,
                OwnerId = "Maarten",
                ScheduledStartTime = DateTime.Now.AddMinutes(10),
            };
            var s2 = new Session()
            {
                Cards = cards,
                PlayerIds = players,
                ThemeId = themeId,
                TurnTime = 30,
                PickTime= 600,
                Phase = SessionPhase.Planned,
                OwnerId = "Maarten",
                ScheduledStartTime = DateTime.Now.AddHours(1),
                ScheduledEndTime = DateTime.Now.AddHours(2)
            };
            var s3 = new Session()
            {
                Cards = cards,
                PlayerIds = players,
                ThemeId = themeId,
                TurnTime = 30,
                PickTime= 600,
                Phase = SessionPhase.Planned,
                OwnerId = "Maarten",
                ScheduledStartTime = DateTime.Now.AddMinutes(-1),
                ScheduledEndTime = DateTime.Now.AddHours(2)
            };
            repo.AddSession(new SessionDto(s1));
            repo.AddSession(new SessionDto(s2));
            repo.AddSession(new SessionDto(s3));*/
        }

        public bool LoadSession(Guid sessionId)
        {
            var session = GetSession(sessionId);
            if (session == null)
            {
                logger.LogDebug("LoadSession niet gevonden " + sessionId.ToString());
                return false;
            }

            var state = new SessionState(session);
            if (session.Phase == SessionPhase.Planned)
            {
                // Moet er hier iets gebeuren?
                logger.LogDebug("Nieuwe sessie starten " + sessionId.ToString());
            }
            else if (session.Phase == SessionPhase.Active)
            {
                logger.LogInformation("Herstellen van sessie uit DB " + sessionId.ToString());
                session.SessionEvents.ToList().ForEach(e => {
                    logger.LogDebug("Applyen event type: " + e.Type);
                    ProcessEvent(state, e);
                });
            }
            else
            {
                logger.LogError("Ongeldige session phase waarde " + sessionId.ToString());
                return false;
            }

            ActiveSessions[sessionId] = state;
            return true;
        }

        public string RegisterClient(Guid sessionId, string token)
        {
            var state = GetSessionState(sessionId);
            if (state == null)
            {
                return null;
            }

            var user = services.VerifyToken(token).Result;

            if (user == null)
            {
                return null;
            }
            else
            {
                var p = state.Players.Find(u => u.UserName == user);
                if (p == null)
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }
        }

        public bool IsSessionStartable(ISession session, out string message)
        {
            message = "None";
            if (session.PlayerIds.Count() <= 1)
            {
                message = "Niet genoeg spelers";
                return false;
            }

            // TODO: meer checks

            return true;
        }
        public bool IsSessionStartable(Guid sessionId, out string message)
        {
            var session = GetSession(sessionId);
            return IsSessionStartable(session, out message);
        }

        public bool IsManager(Guid sessionId, string user)
        {
            try
            {
                var session = GetSession(sessionId);
                if (session == null)
                {
                    logger.LogDebug("IsManager - sessie bestaat niet " + sessionId.ToString());
                    return false;
                }

                var theme = services.GetTheme(session.ThemeId).Result;
                if (theme == null)
                {
                    logger.LogDebug("IsManager - thema NULL " + sessionId.ToString());
                    return false;
                }

                var mgr = (theme.Username == user || theme.Organizers.Contains(user)); 
                return mgr;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);
                return false;
            }
       }

        public User GetUser(Guid sessionId, string userId)
        {
            var state = GetSessionState(sessionId);
            if (state == null)
            {
                return null;
            }

            var user = state.Players.Find(u => u.UserName == userId);
            return user;
        }

        public bool CanMove(Guid sessionId, string userId, string cardId)
        {
            var state = GetSessionState(sessionId);

            // Check of de gebruiker aan de beurt is
            if (state.GameStarted == false || state.CurrentPlayer.UserName != userId)
            {
                return false;
            }  

            try
            {
                var cp = state.CardPositions[cardId];
                // Check of de kaart al op laatste ring staat
                if (cp == 5)
                {
                    return false;
                }
            }
            catch (KeyNotFoundException ex)
            {
                var e = ex;

                // Kaart bestaat niet
                return false;
            }

            return true;
        }

        public ISession GetSession(Guid sessionId)
        {
            var result = repository.GetSession(sessionId).Result;
            if (result == null)
            {
                return null;
            }
            return new Session(result);
        }

        public List<string> GetExpiredTurnSessions()
        {
            var results = new List<string>();
            var now = DateTime.Now;
            foreach (var state in ActiveSessions.Values)
            {
                if (state.GameStarted  && state.Session.Phase == SessionPhase.Active && now.CompareTo(state.TurnStartTime.AddSeconds(state.Session.TurnTime)) >= 0)
                {
                    results.Add(state.SessionId);
                }
            }
            return results;
        }

        public SessionState GetSessionState(Guid sessionId)
        {
            try
            {
                var s = ActiveSessions[sessionId];
                return s;
            }
            catch (KeyNotFoundException ex)
            {
                var e = ex;
                return null;
            }
        }


        public bool CheckCardPickingFinished(Guid sessionId)
        {
            var state = GetSessionState(sessionId);
            return (state.CardPicks.Count == state.Players.Count);
        }
        
        public bool CanPick(Guid sessionId, string userId, string[] cardIds)
        {
            var state = GetSessionState(sessionId);

            if (cardIds.Length == 0)
            {
                logger.LogDebug("{0} heeft geen kaarten gekozen", userId);
                return false;
            }

            // Check of gebruiker al kaarten gekozen heeft
            if (state.CardPicks.ContainsKey(userId))
            {
                logger.LogDebug(userId + " heeft al kaarten gekozen");
                return false;
            }

            // Check of alle card IDs geldig zijn
            return cardIds.All(cid => state.Session.Cards.Any(c => c.CardId == cid));
        }

        public List<string> MergePicks(Guid sessionId)
        {
            var state = GetSessionState(sessionId);
            if (state == null) 
            {
                logger.LogInformation("Sessie bestaat niet of niet geladen: " + sessionId.ToString());
                return null;
            }

            var cardIds = new List<string>();
            foreach (var picks in state.CardPicks.Values)
            {
                foreach (var pick in picks)
                {
                    cardIds.Add(pick);
                }
            }
            return cardIds.Distinct().ToList();
        }

        // State updates adhv events doorvoeren
        // Validatie gebeurt voordat het event gegenereerd wordt dus hier moet 
        // niets meer gecontroleerd worden, enkel de updates doorvoeren
        public void ProcessEvent(SessionState state, ISessionEvent e)
        {
            var mutex = new Mutex(false, state.SessionId);
            try 
            {
                mutex.WaitOne();
                if (e is ChatMessageEvent)
                {
                    var cme = (ChatMessageEvent)e;
                    state.ChatLogs.Add(new ChatMessage()
                    {
                        Timestamp = cme.Timestamp,
                        UserId = cme.UserId,
                        Message = cme.Message
                    });
                }
                else if (e is MoveEvent)
                {
                    var me = (MoveEvent)e;
                    var pos = state.CardPositions[me.CardId];
                    state.CardPositions[me.CardId] = pos + 1;
                }
                else if (e is TurnStartEvent)
                {
                    state.TurnNr = e.TurnNr;
                    state.TurnStartTime = DateTime.Now;
                }
                else if (e is SessionStartEvent)
                {
                    var sse = (SessionStartEvent)e;
                    state.Session = sse.Session;
                    state.Players = sse.Players;
                }
                else if (e is CardPickEvent)
                {
                    var cpe = (CardPickEvent)e;
                    state.CardPicks[e.UserId] = cpe.Cards; 
                }
                else if (e is GameStartEvent)
                {
                    // Mappen naar Card entiteiten uit de session
                    var gse = (GameStartEvent)e;
                    state.Cards = gse.Cards
                        .Select(cid => state.Session.Cards.Where(c => c.CardId == cid).FirstOrDefault())
                        .ToList();
                    // Kaarten op buitenste ring plaatsen
                    state.Cards.ForEach(c => state.CardPositions[c.CardId] = 1);
                    // Echte spel begint
                    state.GameStarted = true;
                    state.TurnNr = 1;
                    state.TurnStartTime = gse.Timestamp;
                }
                else if (e is SessionEndEvent)
                {
                    var sse = (SessionEndEvent)e;
                    state.Session.Phase = SessionPhase.Finished;
                }
                else
                {
                    throw new NotImplementedException(e.Type);
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public bool PersistEvent(ISessionEvent e)
        {
            var result = repository.AddSessionEvent(new SessionEventDto
            {
                SessionId = e.SessionId,
                SessionEvent = e
            }).Result;
            return result.IsAcknowledged;
        }

        public List<ISession> GetStartingSessions()
        {
            var res = repository.GetStartingSessions().Result;
            var dtos = new List<SessionDto>(res);
            var sessions = new List<ISession>();
            dtos.ForEach(dto => sessions.Add(new Session(dto)));
            return sessions;
        }

        public List<ISession> GetActiveSessions()
        {
            var res = repository.GetActiveSessions().Result;
            var dtos = new List<SessionDto>(res);
            var sessions = new List<ISession>();
            dtos.ForEach(dto => sessions.Add(new Session(dto)));
            dtos.Select(dto => new Session(dto)).ToList();
            return sessions;
        }

        public List<ISession> GetEndingSessions()
        {
            // TODO: implementeren
            return null;
        }

        public bool UpdateSessionPhase(Guid sessionId, SessionPhase phase)
        {
            var res = repository.UpdateSessionPhase(sessionId, phase).Result;
            return res.IsAcknowledged;
        }

        public List<User> GetSessionUsers(ISession session)
        {
            // TODO: deftig implementeren
            return session.PlayerIds.Select(pid => new User()
            {
               UserName = pid 
            }).ToList();
        }

        public bool AddSnapshot(Guid sessionId, string description, bool userCreated = false)
        {
            var state = GetSessionState(sessionId);
            if (state == null)
            {
                logger.LogInformation("Kan geen snapshot maken van session die niet geladen is... " + sessionId.ToString());
                return false;
            }
            var snapshot = new Snapshot()
            {
                TurnNr = state.TurnNr,
                UserCreated = userCreated,
                Description = description
            };
            var result = repository.AddSessionSnapshot(sessionId, snapshot).Result;
            return result.IsAcknowledged;
        }
    }
}
