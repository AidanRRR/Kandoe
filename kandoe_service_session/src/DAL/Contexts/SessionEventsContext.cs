using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Configurations;
using Microsoft.Extensions.Options;
using Models.Models.Events;
using Models.Models.Events.Dto;
using Models.Models.Imported;
using MongoDB.Driver;

namespace DAL.Contexts
{
    public interface ISessionEventsContext
    {
        IMongoCollection<SessionDto> SessionEvents { get; }
    }

    public class SessionEventsContext : ISessionEventsContext
    {
        private readonly IMongoDatabase _database = null;

        public SessionEventsContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);


            // VS Code Debugging
            //var client = new MongoClient("mongodb://localhost:27017");
            //_database = client.GetDatabase("SessionsDb");
        }

        public IMongoCollection<SessionDto> SessionEvents => _database.GetCollection<SessionDto>("SessionEvents");
    }
}
