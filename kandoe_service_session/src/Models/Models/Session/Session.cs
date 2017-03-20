using System.Collections.Generic;
using System;
using Models.Models.Imported;
using Models.Models.Events;
using Models.Models.Events.Dto;

namespace Models.Models.Session
{
    public class Session: ISession
    {
        public Session()
        {
            this.ReplayKey = Guid.NewGuid(); 
        }
        
        public Session(SessionDto dto)
        {
            this.SessionId = dto.SessionId;
            this.ThemeId = dto.ThemeId;
            this.Name = dto.Name;
            this.Description = dto.Description;
            this.ReplayKey = dto.ReplayKey;
            this.CircleType = dto.CircleType;
            this.Cards = dto.Cards;
            this.InvitedUserEmails = dto.InvitedUserEmails;
            this.PlayerIds = dto.PlayerIds;
            this.TurnTime = dto.TurnTime;
            this.PickTime = dto.PickTime;
            this.MinPicks = dto.MinPicks;
            this.MaxPicks = dto.MaxPicks;
            this.OwnerId = dto.OwnerId;
            this.ManagerIds = dto.ManagerIds;
            this.SessionEvents = dto.SessionEvents;
            this.SessionSnapshots = dto.SessionSnapshots;
            this.Phase = dto.Phase;
            this.ScheduledEndTime = dto.ScheduledEndTime;
            this.ScheduledStartTime = dto.ScheduledStartTime;
        }

        public Guid SessionId { get;set; }
        public Guid ThemeId { get; set; }
        public string Name {get; set;}
        public string Description {get; set;}
        public Guid ReplayKey { get; set; }
        public CircleType CircleType { get; set; }
        public IEnumerable<Card> Cards { get; set; }
        public IEnumerable<string> InvitedUserEmails { get; set; } = new List<string>();
        public IEnumerable<string> PlayerIds { get; set; } = new List<string>();
        public int TurnTime { get; set; }
        public int PickTime { get; set; }
        public int MinPicks {get; set;}
        public int MaxPicks { get; set; }
        public string OwnerId { get ; set; }
        public IEnumerable<string> ManagerIds { get; set; }
        public IEnumerable<ISessionEvent> SessionEvents { get; set; } = new List<ISessionEvent>();
        public IEnumerable<ISnapshot> SessionSnapshots { get; set; } = new List<ISnapshot>();
        public SessionPhase Phase { get; set; }
        public DateTime? ScheduledStartTime { get; set; }
        public DateTime? ScheduledEndTime { get; set; }        
    }
}