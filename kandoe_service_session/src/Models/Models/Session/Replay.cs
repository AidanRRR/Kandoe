using System.Collections.Generic;
using Models.Models.Events;

namespace Models.Models.Session
{
    public class Replay: IReplay
    {
        public ISession Session { get; set; }
        public IEnumerable<ISessionEvent> Events { get; set; }
        public IEnumerable<ISnapshot> Snapshots { get; set; }
    }
}