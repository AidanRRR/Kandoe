using System.Collections.Generic;
using Models.Models.Imported;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Models.Models.API;
using Models.Models;
using DAL.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BL
{
    public class ServiceManager: IServiceManager
    {
        public class ServiceManagerConfig
        {
            public string ThemeServiceUrl { get; set; }
            public string AuthServiceUrl { get; set; }
            public string UserServiceUrl { get; set; }
        }

        private ISessionEventsRepository repository;
        private ILogger logger;
        private ServiceManagerConfig config;
        public ServiceManager(ISessionEventsRepository repository, ILogger<ServiceManager> logger, IOptions<ServiceManagerConfig> config)
        {
            this.repository = repository;
            this.logger = logger;
            this.config = config.Value;
        }

        public async Task<ITheme> GetTheme(Guid themeId)
        {
            ITheme theme = null;
            logger.LogDebug("GetTheme " + themeId.ToString());
            using (var http = new HttpClient())
            {
                http.Timeout = TimeSpan.FromSeconds(5);

                HttpResponseMessage res;
                try
                {
                    res = await http.GetAsync(config.ThemeServiceUrl + "/api/Theme/GetTheme/" + themeId.ToString());
                }
                catch (Exception ex)
                {
                    logger.LogInformation("HTTP GetTheme call mislukt");
                    logger.LogInformation(ex.ToString());
                    return null;
                }

                if (!res.IsSuccessStatusCode)
                {
                    logger.LogInformation("HTTP GetTheme geen 200 status code");
                    return null;
                }
                
                try
                {
                    var content = res.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<ApiResult<Theme>>(content);
                    theme = result.Data;
                }
                catch (Exception ex)
                {
                    logger.LogInformation("Serialisatiefout: " + ex.Message);
                }
            }
            return theme;
        }

        public async Task<string> VerifyToken(string jwtToken)
        {
            logger.LogDebug("Verify JWT token " + jwtToken);
            using (var http = new HttpClient())
            {
                http.Timeout = TimeSpan.FromSeconds(5);
                http.DefaultRequestHeaders.Add("x-acces-token", jwtToken);

                HttpResponseMessage res;
                try
                {

                    res = await http.PostAsync(config.AuthServiceUrl + "/api/auth/verify", new StringContent(""));
                }
                catch (Exception ex)
                {
                    logger.LogInformation("HTTP verify call mislukt");
                    logger.LogInformation(ex.ToString());
                    return null;
                }

                if (!res.IsSuccessStatusCode)
                {
                    logger.LogInformation("HTTP verify call geen 200 status code");
                    logger.LogInformation(res.StatusCode + ": " + res.ReasonPhrase);
                    return null;
                }
                
                var content = res.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<VerifyResult>(content);
                
                if (!result.Success)
                {
                    logger.LogDebug("Geen geldig token");
                    return null;
                }
                else
                {
                    return result.Decoded.Name;
                }
            }
        }

        public async Task<bool> IsTokenManager(Guid sessionId, string jwt)
        {
            var user = await VerifyToken(jwt);
            if (user == null)
            {
                return false;
            }

            var session = await repository.GetSession(sessionId);
            if (session == null)
            {
                return false;
            } 

            var theme = await GetTheme(session.ThemeId);
            if (theme == null)
            {
                return false;
            }

            return (theme.Username == user || theme.Organizers.Contains(user));
        }

        public async Task<User> GetUser(string userName)
        {
            logger.LogDebug("Get User: " + userName);
            try
            {
                if (userName == null || userName.Length == 0)
                {
                    return null;
                }

                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(5);
                    var res = await http.GetAsync(config.UserServiceUrl + "/api/Users/GetUserById/" + userName);

                    if (!res.IsSuccessStatusCode)
                    {
                        logger.LogInformation("HTTP getuser geen 200 status code");
                        logger.LogInformation(config.UserServiceUrl + "/api/Users/GetUserById/" + userName);
                        logger.LogInformation(res.StatusCode + ": " + res.ReasonPhrase);
                        return null;
                    }
                    
                    var content = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResult<User>>(content);
                    return result.Data;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Ophalen user data mislukt");
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsers(string[] userNames)
        {
            logger.LogDebug("Get users: "  + userNames.ToString());
            try
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(5);

                    var res = await http.GetAsync(config.UserServiceUrl + "/api/Users");

                    if (!res.IsSuccessStatusCode)
                    {
                        logger.LogInformation("HTTP getusers call geen 200 status code");
                        logger.LogInformation(res.StatusCode + ": " + res.ReasonPhrase);
                        return null;
                    }
                    
                    var content = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResult<IEnumerable<User>>>(content);
                    return result.Data;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Ophalen user data mislukt");
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}