
using System;
using System.Collections.Generic;
using Models.Models.Events;
using Models.Models.Session;
using Models.Models.Imported;
using System.Threading.Tasks;

namespace BL
{
    public interface ISessionManager
    {
        // Laden van een session in het geheugen
        // Gebruikers kunnen enkel connecten met geladen sessions
        bool LoadSession(Guid sessionId);
        // Controleer of een gebruiker met dit JWT token kan
        // deelnemen aan de sessie. Geeft gebruiker ID terug 
        // of NULL als de gebruiker niet mag connecten
        string RegisterClient(Guid sessionId, string token);
        // Controleren of gebruiker manager is van session
        bool IsManager(Guid sessionId, string user);
        // Ophalen user object uit session state
        User GetUser(Guid sessionId, string userId);
        // Controleer of een gebruiker in een bepaalde session een
        // bepaalde kaart mag verschuiven
        bool CanMove(Guid sessionId, string userId, string cardId);
        // Check of iedereen kaarten heeft gekozen
        bool CheckCardPickingFinished(Guid sessionId);
        // Kan gebruiker kaarten kiezen?
        bool CanPick(Guid sessionId, string userId, string[] cardIds);
        // Check of een sessie in een geldige state is om te starten.
        bool IsSessionStartable(ISession session, out string message);
        bool IsSessionStartable(Guid sessionId, out string message);
        List<User> GetSessionUsers(ISession session);
        // Controleer bij alle actieve sessies of de beurt van de
        // actieve speler is verlopen
        List<string> GetExpiredTurnSessions();
        // Ophalen session state adhv de ID
        SessionState GetSessionState(Guid sessionId);
        // Update de state van een session adhv een event
        void ProcessEvent(SessionState state, ISessionEvent e);
        // Lezen van een session uit de databank
        ISession GetSession(Guid sessionId);
        // Opslagen van een event in de databank
        // Samenvoegen van card picks van alle spelers
        List<string> MergePicks(Guid sessionId);
        bool PersistEvent(ISessionEvent e);
        // Lijst van sessies die nog niet gestart zijn maar waarvan het
        // geplande start tijdstip aangebroken is
        List<ISession> GetStartingSessions();
        // Lijst van actieve sessies
        List<ISession> GetActiveSessions();
        List<ISession> GetEndingSessions();
        // Updaten van de session phase in de databank
        bool UpdateSessionPhase(Guid sessionId, SessionPhase phase);
        // Toevoegen van een snapshot aan een session
        bool AddSnapshot(Guid sessionId, string description, bool userCreated);
    }
}
