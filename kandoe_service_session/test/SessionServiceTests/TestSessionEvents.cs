using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API;
using BL;
using DAL.Repositories;
using Models.Models.Events;
using Xunit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DAL.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Models.Models.Session;
using Models.Models.Imported;

namespace SessionServiceTests
{
    public class TestSessionEvents
    {
        private ISessionEventsRepository repo;
        private ISessionManager mgr;
        private Session session;

        public TestSessionEvents()
        {
            var opts = Options.Create<MongoSettings>(new MongoSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                Database = "SessionsDb"
            });
            repo = new SessionEventsRepository(opts);
            mgr = new SessionManager(repo); 

             var cards = new Card[]
            {
                new Card()
                {
                    CardId = "a",
                    ImageUrl = "assets/bier.jpg",
                    Text = "Bier",
                    ThemeId = "a"
                },
                new Card()
                {
                    CardId = "b",
                    ImageUrl = "assets/koffie.jpg",
                    Text = "Koffie",
                    ThemeId = "a"
                },
                new Card()
                {
                    CardId = "c",
                    ImageUrl = "assets/thee.jpg",
                    Text = "Thee",
                    ThemeId = "a"
                },
                new Card()
                {
                    CardId = "d",
                    ImageUrl = "assets/Water.jpg",
                    Text = "Water",
                    ThemeId = "a"
                },
                new Card()
                {
                    CardId = "e",
                    ImageUrl = "assets/Wijn.jpg",
                    Text = "Wijn",
                    ThemeId = "a"
                },
            };
            var players = new string[]
            {
                "Maarten",
                "Vincent",
                "Aidan"
            };

            session = new Session()
            {
                //SessionId = Guid.Parse("3dcf13e6-8ce3-4e37-9f36-51e857dc5e21"),
                Cards = cards,
                PlayerIds = players,
                TurnTime = 1,
                PickTime= 180,
                Phase = SessionPhase.Planned,
                ScheduledStartTime = DateTime.Now,
            };
        }

        public SessionState CreateState()
        {
           

            var state =  new SessionState(session)
            {
                TurnNr = 2,
                TurnStartTime = DateTime.Now,
                Cards = session.Cards.ToList()
            };

            session.PlayerIds.ToList().ForEach(p => 
            {
                state.Players.Add(new User() 
                { 
                    UserName = p 
                });
            });
            state.Cards.ForEach(c => 
            {
                state.CardPositions[c.CardId] = 1;
            });

            return state;
        }
        
        [Fact]
        public void TestChatMessageEvent()
        {
            var s1 = CreateState();
            var s2 = CreateState();
            var e = new ChatMessageEvent()
            {
                TurnNr = s1.TurnNr,
                Message = "test message",
                UserId = "Maarten"
            };
            
            Assert.Empty(s1.ChatLogs);

            mgr.ProcessEvent(s1, e);

            // Chat Message toegevoegd
            Assert.NotEmpty(s1.ChatLogs);
            var cl = s1.ChatLogs[0];
            Assert.Equal(cl.UserId, e.UserId);
            Assert.Equal(cl.Message, e.Message);

            // Rest van de state niet verandert
            Assert.Equal(s1.TurnNr, s2.TurnNr);
            Assert.All(s1.CardPositions.AsEnumerable(), kv => s1.CardPositions[kv.Key].Equals(s2.CardPositions[kv.Key]));
        }

        [Fact]
        public void TestMoveEvent()
        {
            var s1 = CreateState();
            var s2 = CreateState();
            var cardId = "a";
            var e = new MoveEvent()
            {
                TurnNr = s1.TurnNr,
                UserId = "Maarten",
                CardId = cardId,
            };
            
            Assert.Equal(s1.CardPositions[cardId], 1);

            mgr.ProcessEvent(s1, e);

            // Kaart positie geupdate
            s1.CardPositions.ToList().ForEach(kv =>
            {
                if (kv.Key.Equals(cardId))
                {
                    Assert.Equal(s1.CardPositions[kv.Key], 2);
                }
                else
                {
                    Assert.Equal(s1.CardPositions[kv.Key], s2.CardPositions[kv.Key]);
                }
            });

            // Rest van state niet verandert
            Assert.Equal(s1.TurnNr, s2.TurnNr);
            Assert.Equal(s1.ChatLogs.Count, s2.ChatLogs.Count);
        }

        [Fact]
        public void TestTurnStartEvent()
        {
            var s1 = CreateState();
            var s2 = CreateState();
            var e = new TurnStartEvent()
            {
                TurnNr = s1.TurnNr + 1,
                UserId = "Aidan"
            };

            Assert.Equal(s1.CurrentPlayer.UserName, s2.CurrentPlayer.UserName);
            Assert.Equal(s1.TurnNr, s2.TurnNr);

            mgr.ProcessEvent(s1, e);

            Assert.Equal(s1.TurnNr, s2.TurnNr + 1);
            Assert.NotEqual(s1.CurrentPlayer.UserName, s2.CurrentPlayer.UserName);

            // Rest van state niet verandert 
            Assert.Equal(s1.ChatLogs.Count, s2.ChatLogs.Count);
            Assert.All(s1.CardPositions.AsEnumerable(), kv => s1.CardPositions[kv.Key].Equals(s2.CardPositions[kv.Key]));
        }

        [Fact]
        public void TestCardPickEvent()
        {
           var state = new SessionState(session);  

           Assert.False(state.CardPicks.ContainsKey("Maarten"));
           Assert.False(state.CardPicks.ContainsKey("Vincent"));

           var pick1 = new CardPickEvent()
           {
                SessionId = session.SessionId,
                UserId = "Maarten",
                Cards = new string[] { "a", "c" }
           };
           mgr.ProcessEvent(state, pick1);
           Assert.True(state.CardPicks.ContainsKey("Maarten"));
           Assert.False(state.CardPicks.ContainsKey("Vincent"));
           Assert.Equal(state.CardPicks["Maarten"][0], "a");
           Assert.Equal(state.CardPicks["Maarten"][1], "c");

           var pick2 = new CardPickEvent()
           {
                SessionId = session.SessionId,
                UserId = "Vincent",
                Cards = new string[] { "c", "a" }
           };
           mgr.ProcessEvent(state, pick2);
           Assert.True(state.CardPicks.ContainsKey("Maarten"));
           Assert.Equal(state.CardPicks["Maarten"][0], "a");
           Assert.Equal(state.CardPicks["Maarten"][1], "c");
           Assert.True(state.CardPicks.ContainsKey("Vincent"));
           Assert.Equal(state.CardPicks["Vincent"][0], "c");
           Assert.Equal(state.CardPicks["Vincent"][1], "a");
        }

        [Fact]
        public void TestGameStartEvent()
        {
            var state = new SessionState(session);

            Assert.Empty(state.Cards);

            var gse = new GameStartEvent()
            {
                SessionId = session.SessionId,
                Cards = new string[] { "a", "b", "d" }
            };
            mgr.ProcessEvent(state, gse);
            Assert.Equal(3, state.Cards.Count);
        }

        [Fact]
        public void TestSessionStartEvent()
        {
            var state = new SessionState(session);

            Assert.Empty(state.Players); 

            var sse = new SessionStartEvent()
            {
                SessionId = session.SessionId,
                Session = session,
                Players = new List<User>(session.PlayerIds.Select(id => new User()
                {
                    UserName = id
                }))
            };
            mgr.ProcessEvent(state, sse);
            Assert.Equal(state.Players.Count, 3);
        }
    }
}