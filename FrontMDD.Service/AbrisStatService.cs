using FrontMDD.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrontMDD.Service
{
    public class AbrisStatServices
    {
        private readonly HttpClient _httpClient;

        public AbrisStatServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetAbrisStat(string id, string dateStart, string dateEnd)
        {
            string api = "https://apitestg2.azurewebsites.net/api";
            string apiLocal = "https://localhost:7058/api";

            Console.WriteLine(api);

            var content = new StringContent(
                JsonConvert.SerializeObject(new { Id = id, DateStart = dateStart, DateEnd = dateEnd }),
                Encoding.UTF8,
                "application/json"
            );

            using var response = await _httpClient.PostAsync(api + "/ShelterState/Stat", content);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var stat = JsonConvert.DeserializeObject<int>(json);

            return stat;
        }

    }
}
