using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Configurations;
using DAL.Contexts;
using Microsoft.Extensions.Options;
using Models.Models;
using Models.Models.API;
using Models.Models.Users;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> GetAllUsersByIds(IEnumerable<string> userNames);
        Task<User> GetUser(string id);
        Task<User> GetUserByEmail(string email);
        Task<User> AddUser(User user);
        Task<UpdateResult> DisableUser(string id);
        Task<User> UpdateUser(string id, User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly UsersContext _context = null;

        public UserRepository(IOptions<MongoSettings> settings)
        {
            _context = new UsersContext(settings);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(e => e.IsEnabled, true);

            return await _context.Users.Find(filter).ToListAsync();
        }

        public async Task<User> GetUser(string id)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(e => e.UserName, id) & builder.Eq(e => e.IsEnabled, true);

            var user = await _context.Users
                .Find(filter)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(e => e.Email, email) & builder.Eq(e => e.IsEnabled, true);

            var user = await _context.Users
                .Find(filter)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> AddUser(User user)
        {
            await _context.Users.InsertOneAsync(user);

            return user;
        }

        public async Task<UpdateResult> DisableUser(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserName, id);
            var update = Builders<User>.Update
                .Set(u => u.IsEnabled, false)
                .CurrentDate(u => u.UpdatedOn);

            return await _context.Users.UpdateOneAsync(filter, update);
        }

        public async Task<User> UpdateUser(string id, User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserName, id);
            var update = Builders<User>.Update
                .Set(u => u.FirstName, user.FirstName)
                .Set(u => u.LastName, user.LastName)
                .Set(u => u.Email, user.Email)
                .Set(u => u.UserName, user.UserName)
                .Set(u => u.Notifications, user.Notifications)
                .CurrentDate(u => u.UpdatedOn);

            await _context.Users.UpdateOneAsync(filter, update);

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersByIds(IEnumerable<string> userNames)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.In(e => e.UserName, userNames) & builder.Eq(e => e.IsEnabled, true);

            var userDto = await _context.Users
                .Find(filter)
                .ToListAsync();

            return userDto;
        }


    }
}