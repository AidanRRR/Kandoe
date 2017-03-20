using System.Collections.Generic;
using Models.Models.Events;
using Models.Models.Events.Dto;

namespace Models.Models.Session
{
    public interface IReplay
    {
        ISession Session { get; set; }
        IEnumerable<ISessionEvent> Events { get; set; }
        IEnumerable<ISnapshot> Snapshots { get; set; }
    }
}