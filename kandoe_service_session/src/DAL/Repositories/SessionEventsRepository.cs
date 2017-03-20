using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL.Configurations;
using DAL.Contexts;
using Microsoft.Extensions.Options;
using Models.Models.Events.Dto;
using MongoDB.Driver;
using MongoDB.Bson;
using Models.Models.Session;

namespace DAL.Repositories
{
    public interface ISessionEventsRepository
    {
        Task<SessionDto> GetSession(Guid sessionId);
        Task<IEnumerable<SessionDto>> GetAllSessions();
        Task<IEnumerable<SessionDto>> GetThemeSessions(Guid themeId);
        Task<UpdateResult> AddSessionEvent(SessionEventDto sessionEvent);
        Task<SessionDto> AddSession(SessionDto session);
        Task<UpdateResult> UpdateSessionPhase(Guid sessionId, SessionPhase phase);
        Task<UpdateResult> AddSessionSnapshot(Guid sessionId, ISnapshot snapshot);
        Task<IEnumerable<SessionDto>> GetStartingSessions();
        Task<IEnumerable<SessionDto>> GetActiveSessions();
        Task<UpdateResult> InviteUsersToSession(Guid sessionId, string email);
        Task<UpdateResult> AcceptSessionInvite(Guid sessionId, string userName, string email);
        Task<IEnumerable<string>> GetActiveSessionsUserParticipates(string UserName);
        Task<IEnumerable<SessionDto>> GetParticipatingSessions(string user);
        Task<IEnumerable<SessionDto>> GetInvitedSessions(string email);
    }

    public class SessionEventsRepository : ISessionEventsRepository
    {
        private SessionEventsContext _context;

        public SessionEventsRepository(IOptions<MongoSettings> settings)
        {
            _context = new SessionEventsContext(settings);
        }

        public async Task<IEnumerable<SessionDto>> GetAllSessions()
        {
            return await _context.SessionEvents.Find(_ => true).ToListAsync();
        }

        public async Task<SessionDto> GetSession(Guid sessionId)
        {
            var filter = Builders<SessionDto>.Filter
                .Eq(e => e.SessionId, sessionId);

            var sessionEvents = await _context.SessionEvents
                .Find(filter)
                .FirstOrDefaultAsync();

            return sessionEvents;            
        }

        public async Task<IEnumerable<SessionDto>> GetParticipatingSessions(string user)
        {
            var filter = Builders<SessionDto>.Filter.AnyEq(e => e.PlayerIds, user);
            var sessions = await _context.SessionEvents
                .Find(filter)
                .ToListAsync();
            return sessions;
        }

        public async Task<IEnumerable<SessionDto>> GetInvitedSessions(string email)
        {
            var filter = Builders<SessionDto>.Filter.AnyEq(e => e.InvitedUserEmails, email);
            var sessions = await _context.SessionEvents
                .Find(filter)
                .ToListAsync();
            return sessions;
        }

        public async Task<IEnumerable<SessionDto>> GetThemeSessions(Guid themeId)
        {
            var filter = Builders<SessionDto>.Filter
                .Eq(e => e.ThemeId, themeId);

            var themeSessions = await _context.SessionEvents
                .Find(filter)
                .ToListAsync();

            return themeSessions;            
        }

        public async Task<SessionDto> AddSession(SessionDto session)
        {
            await _context.SessionEvents.InsertOneAsync(session);

            return session;
        }

        public async Task<UpdateResult> AddSessionEvent(SessionEventDto sessionEvent)
        {
            var filter = Builders<SessionDto>
                .Filter.Eq(e => e.SessionId, sessionEvent.SessionId);

            var update = Builders<SessionDto>
                .Update
                .Push(e => e.SessionEvents, sessionEvent.SessionEvent);

            var updateResult = await _context.SessionEvents.UpdateOneAsync(filter, update);
            
            return updateResult;
        }

        public async Task<UpdateResult> UpdateSessionPhase(Guid sessionId, SessionPhase phase)
        {
            var filter = Builders<SessionDto>.Filter.Eq(e => e.SessionId, sessionId);
            var update = Builders<SessionDto>.Update.Set(e => e.Phase, phase);

            var result = await _context.SessionEvents.UpdateOneAsync(filter, update);
            return result;
        }

        public async Task<UpdateResult> AddSessionSnapshot(Guid sessionId, ISnapshot snapshot)
        {
            var filter = Builders<SessionDto>
                .Filter.Eq(e => e.SessionId, sessionId);

            var update = Builders<SessionDto>
                .Update
                .Push(e => e.SessionSnapshots, snapshot);

            var updateResult = await _context.SessionEvents.UpdateOneAsync(filter, update);
            return updateResult;
        }

        public async Task<IEnumerable<string>> GetActiveSessionsUserParticipates(string UserName) {
            //var activeSessions = await GetActiveSessions();
            var sessions = await GetAllSessions();
            var sessionsWithPlayer = new List<string>();
            
            foreach (var session in sessions)
            {
                if (session.PlayerIds.Contains(UserName))
                {
                    sessionsWithPlayer.Add(session.SessionId.ToString());
                }
            }            

            return sessionsWithPlayer;
        }

        public async Task<IEnumerable<SessionDto>> GetStartingSessions()
        {
            var now = DateTime.Now;
            var field = new StringFieldDefinition<SessionDto, BsonDateTime>("ScheduledStartTime");
            var filter = Builders<SessionDto>.Filter.And(new FilterDefinition<SessionDto>[]
                {
                    Builders<SessionDto>.Filter.Eq(s => s.Phase, SessionPhase.Planned),
                    Builders<SessionDto>.Filter.Lte(field, new BsonDateTime(now))
                }
            );

            var results = await _context.SessionEvents.Find(filter).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<SessionDto>> GetActiveSessions()
        {
            var filter = Builders<SessionDto>.Filter.Eq(s => s.Phase, SessionPhase.Active);
            var results = await _context.SessionEvents.Find(filter).ToListAsync();

            return results;
        }

        public async Task<UpdateResult> InviteUsersToSession(Guid sessionId, string email)
        {
            var filter = Builders<SessionDto>
                .Filter.Eq(e => e.SessionId, sessionId);

            var update = Builders<SessionDto>
                .Update
                .Push(e => e.InvitedUserEmails, email);

            var updateResult = await _context.SessionEvents.UpdateOneAsync(filter, update);

            return updateResult;
        }

        public async Task<UpdateResult> AcceptSessionInvite(Guid sessionId, string userName, string email)
        {
            var filter = Builders<SessionDto>
                .Filter.Eq(e => e.SessionId, sessionId);

            // Toevoegen van de player ID
            var playerUpdate = Builders<SessionDto>
                .Update
                .Push(e => e.PlayerIds, userName);
            var updateResult = await _context.SessionEvents.UpdateOneAsync(filter, playerUpdate);
            // Verwijderen van invite e-mail
            var inviteUpdate = Builders<SessionDto>
                .Update
                .Pull(e => e.InvitedUserEmails, email);
            var updateResult2 = await _context.SessionEvents.UpdateOneAsync(filter, inviteUpdate);

            return updateResult;
        }
    }
}