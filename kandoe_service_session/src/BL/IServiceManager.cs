using System;
using System.Collections.Generic;
using Models.Models.Imported;
using System.Threading.Tasks;

namespace BL
{
    // Calls naar de andere microservices
    public interface IServiceManager
    {
        // Thema ophalen bij de thema service
        Task<ITheme> GetTheme(Guid themeId);        
        // Token verifieren bij de auth service
        Task<string> VerifyToken(string jwt);
        // Is token manager van sessie?
        Task<bool> IsTokenManager(Guid sessionId, string jwt);
        // User ophalen
        Task<User> GetUser(string userName);
        Task<IEnumerable<User>> GetUsers(string[] userNames);
    }
}