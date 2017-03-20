using DAL.Configurations;
using Microsoft.Extensions.Options;
using Models.Models;
using Models.Models.Users;
using MongoDB.Driver;

namespace DAL.Contexts
{
    public interface IUsersContext
    {
        IMongoCollection<User> Users { get; }
    }

    public class UsersContext : IUsersContext
    {
        private readonly IMongoDatabase _database = null;

        public UsersContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
