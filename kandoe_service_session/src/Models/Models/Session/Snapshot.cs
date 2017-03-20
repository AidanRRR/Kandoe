namespace Models.Models.Session
{
    public class Snapshot: ISnapshot
    {
        public int TurnNr { get; set; }
        public string Description { get; set; }
        public bool UserCreated { get; set; }
    }
}