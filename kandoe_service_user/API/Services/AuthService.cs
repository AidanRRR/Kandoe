using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Models.Models.API;
using Models.Models;
using DAL.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace API.Services
{
    public interface IAuthService
    {
        Task<VerifyResult> VerifyToken(string jwtToken);
    }

    public class AuthService : IAuthService
    {
        private string _authUrl;

        public AuthService(IOptions<JwtConfig> jwtConfig)
        {
            _authUrl = jwtConfig.Value.AuthUrl;
        }

        public async Task<VerifyResult> VerifyToken(string jwtToken)
        {
            using (var http = new HttpClient())
            {
                http.Timeout = TimeSpan.FromSeconds(5);
                http.DefaultRequestHeaders.Add("x-acces-token", jwtToken);

                HttpResponseMessage res;
                try
                {
                    res = await http.PostAsync(_authUrl, new StringContent(""));
                }
                catch (Exception ex)
                {
                    return null;
                }

                var content = res.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<VerifyResult>(content);

                return result;
            }
        }
    }
}
