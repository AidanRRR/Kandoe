using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Models.Models.Imported;
using Models.Models.Session;

namespace Models.Models.Events.Dto
{
    public class SessionDto
    {
        public SessionDto()
        {
        }

        public SessionDto(ISession session)
        {
            this.SessionId = session.SessionId;
            this.ThemeId = session.ThemeId;
            this.Name = session.Name;
            this.Description = session.Description;
            this.ReplayKey = session.ReplayKey;
            this.CircleType = session.CircleType;
            this.Cards = session.Cards;
            this.InvitedUserEmails = session.InvitedUserEmails;
            this.PlayerIds = session.PlayerIds;
            this.TurnTime = session.TurnTime;
            this.PickTime = session.PickTime;
            this.MinPicks = session.MinPicks;
            this.MaxPicks = session.MaxPicks;
            this.OwnerId = session.OwnerId;
            this.ManagerIds = session.ManagerIds;
            this.SessionEvents = session.SessionEvents;
            this.SessionSnapshots = session.SessionSnapshots;
            this.Phase = session.Phase;
            this.ScheduledEndTime = session.ScheduledEndTime;
            this.ScheduledStartTime = session.ScheduledStartTime;
        }

        [BsonId]
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
