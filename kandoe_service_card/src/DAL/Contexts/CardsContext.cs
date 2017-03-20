using DAL.Configurations;
using Microsoft.Extensions.Options;
using Models.Models.Cards;
using MongoDB.Driver;

namespace DAL.Contexts
{
    public class CardsContext
    {
        private readonly IMongoDatabase _database;

        public CardsContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Card> Cards => _database.GetCollection<Card>("Cards");
    }
}
