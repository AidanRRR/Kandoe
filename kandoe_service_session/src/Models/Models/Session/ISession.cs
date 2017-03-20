using System.Collections.Generic;
using System;
using Models.Models.Imported;
using Models.Models.Events;

namespace Models.Models.Session
{
    public interface  ISession
    {
        Guid SessionId { get;set; }
        // Thema
        Guid ThemeId { get; set; }
        // Sleutel om als niet beheerder een replay te bekijken
        // TODO: JsonIgnore 
        Guid ReplayKey { get; set; }
        // Naam van de sessie
        string Name { get; set; }
        // Beschrijving van de sessie
        string Description { get; set; }
        // Kans of probleem?
        CircleType CircleType { get; set; }
        // Kaarten waaruit spelers in het begin kunnen kiezen
        IEnumerable<Card> Cards { get; set; }
        // Email adressen van gebruikers die uitgenodigd zijn (Uitnodiging nog niet geaccepteerd)
        IEnumerable<string> InvitedUserEmails { get ;set; }
        // IDs van alle gebruikers die meespelen (dus uitnodiging geaccepteerd)
        IEnumerable<string> PlayerIds { get; set; }
        // Tijd in seconden dat een gebruiker maximaal mag doen over zijn beurt)
        int TurnTime { get; set; }
        //  Tijd in seconden dat het kiezen van kaarten maximaal mag duren in het begin van het spel
        int PickTime { get; set; }
        // Minimaal aantal kaartjes dat een speler moet kiezen
        int MinPicks {get; set;}
        // Maximaal aantal kaarten dat een speler mag kiezen
        int MaxPicks { get; set; }
        // Eigenaar ID
        string OwnerId { get; set; }
        // User IDs van managers van de sessie
        IEnumerable<string> ManagerIds { get; set; }
        // Chronologische lijst van events
        IEnumerable<ISessionEvent> SessionEvents { get; set; }
        // Snapshots
        IEnumerable<ISnapshot> SessionSnapshots { get; set; }
        // Fase levenscyclus van een sessie
        SessionPhase Phase { get; set; }
        // Gepland tijdstip om de sessie te beginnen
        DateTime? ScheduledStartTime { get; set; }
        // Eind tijdstip
        DateTime? ScheduledEndTime { get; set; }
    }
}