using DAL.Configurations;
using Microsoft.Extensions.Options;
using Models.Models.Themes;
using MongoDB.Driver;

namespace DAL.Contexts
{
    public class ThemesContext
    {
        private readonly IMongoDatabase _database;

        public ThemesContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Theme> Themes => _database.GetCollection<Theme>("Themes");
    }
}
