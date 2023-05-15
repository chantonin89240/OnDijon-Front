using FrontMDD.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace FrontMDD.Service
{
    public class ProfilServices
    {
        private readonly HttpClient _httpClient;

        public ProfilServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Profil>> GetAllProfil()
        {
            string api = "https://apitestg2.azurewebsites.net/api/";
            Console.WriteLine(api);

            using HttpResponseMessage response = await _httpClient.GetAsync(api + "/Profil");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var listProfils = JsonConvert.DeserializeObject<List<Profil>>(json);

            Console.WriteLine(listProfils);
            if (listProfils != null && listProfils.Count > 0)
            {
                return listProfils;

            }
            else
            {
                throw new Exception("Aucun profil trouvé.");

            }

        }

    }
}