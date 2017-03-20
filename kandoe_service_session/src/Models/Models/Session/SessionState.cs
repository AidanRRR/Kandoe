using System;
using System.Collections.Generic;
using Models.Models.Chat;
using Models.Models.Imported;

namespace Models.Models.Session
{
    public class SessionState
    {
        public ISession Session { get; set; }
        public string SessionId 
        { 
            get
            {
                return Session.SessionId.ToString();
            }  
        }
        public List<Card> Cards { get; set; }
        public List<User> Players { get; set; }
        public Dictionary<string, int> CardPositions { get; set; }
        public Dictionary<string, string[]> CardPicks { get; set; }
        public bool GameStarted { get; set; }
        public int TurnNr { get; set; } 
        public DateTime TurnStartTime { get; set; }
        public User CurrentPlayer 
        {
            get {
                return Players[TurnNr % Players.Count];
            }
        }
        public List<ChatMessage> ChatLogs;
        public SessionState(ISession session) 
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            this.Session = session;

            ChatLogs = new List<ChatMessage>(); 
            Cards = new List<Card>();
            Players = new List<User>();
            CardPositions = new Dictionary<string, int>();
            TurnNr = 0;
            GameStarted = false;
            CardPicks = new Dictionary<string, string[]>();
        }
    }
}