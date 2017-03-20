namespace Models.Models.Session
{
    // Een snapshot van een speltoestand gemaakt door de beheerder (manueel)
    // of automatisch door het systeem (na een volledige ronde)
    public interface ISnapshot
    {
        // Beurt in sessie van snapshot
        // Aan de hand hiervan kunnen we de state reconstrueren
        int TurnNr { get; set; }
        string Description { get; set; }
        // Snapshot kan gemaakt worden door het systeem (na ronde) of door beheerder
        bool UserCreated { get; set; }
    }
}