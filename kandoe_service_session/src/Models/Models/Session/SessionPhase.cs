
namespace Models.Models.Session
{
    // Fase levenscyclus van een sessie
    public enum SessionPhase
    {
        // Sessie is nog niet begonnen
        Planned,
        // Sessie loopt momenteel
        Active,
        // Sessie is afgelopen
        Finished
    }
}