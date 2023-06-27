using FrontMDD.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontMDD.Service
{
    public class AbrisServices
    {
        private readonly HttpClient _httpClient;

        public AbrisServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Abris>> GetAllAbris()
        {
            string api = "https://apiprodg2.azurewebsites.net/api";
            string apiLocal = "https://localhost:7058/api";
            Console.WriteLine(api);

            using HttpResponseMessage response = await _httpClient.GetAsync(api + "/Abri");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var listAbris = JsonConvert.DeserializeObject<List<Abris>>(json);

            Console.WriteLine(listAbris);
            if (listAbris != null && listAbris.Count > 0)
            {
                return listAbris;
            }
            else
            {
                throw new Exception("Aucun abris trouvé.");
            }

        }
        public async Task<List<ShelterState>> GetAllShelterState()
        {
            string api = "https://apiprodg2.azurewebsites.net/api";
            string apiLocal = "https://localhost:7058/api";


            using HttpResponseMessage response = await _httpClient.GetAsync(api + "/ShelterState/All");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var listShelterState = JsonConvert.DeserializeObject<List<ShelterState>>(json);

            Console.WriteLine(listShelterState);
            if (listShelterState != null && listShelterState.Count > 0)
            {
                return listShelterState;
            }
            else
            {
                throw new Exception("Aucun abris états trouvé." );
            }

        }

    }
}
