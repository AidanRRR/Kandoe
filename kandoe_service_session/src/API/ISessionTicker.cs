namespace API
{
    public interface ISessionTicker
    {
        // Periodiek controleren of er geplande sessies zijn die moeten opgestart worden
        void ScheduledSessionTick(object arg);
        // Periodiek controleren of er actieve sessies zijn die moeten beeindigd worden
        void EndSessionTick(object arg);
        // Controleer of er sessies zijn die hersteld moeten worden
        void RestoreSessions();
    }
}