using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API.Authentication
{
    public class AuthService
    {
        public async Task<string> VerifyToken(string jwtToken)
        {
            using (var http = new HttpClient())
            {
                http.Timeout = TimeSpan.FromSeconds(5);
                http.DefaultRequestHeaders.Add("x-acces-token", jwtToken);
 
                HttpResponseMessage res;
                try
                {
                    res = await http.PostAsync("http://95.85.12.203:5010/api/Auth/verify", new StringContent(""));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("http verify call mislukt");
                    Console.WriteLine(ex.ToString());
                    return null;
                }
 
                if (!res.IsSuccessStatusCode)
                {
                    Console.WriteLine("http verify call mislukt");
                    return null;
                }
               
                var content = res.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
                var result = JsonConvert.DeserializeObject<VerifyResult>(content);
               
                if (!result.Success)
                {
                    Console.WriteLine("Geen geldig token...");
                    return null;
                }
                else
                {
                    return result.Decoded.Name;
                }
            }
        }
    }
}