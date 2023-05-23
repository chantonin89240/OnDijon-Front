using FrontMDD.Entities;
using FrontMDD.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Text;

namespace FrontMDD.WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AbrisServices _abrisServices;
        private readonly AbrisStatServices _abrisStatServices;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public List<Abris>? Abris { get; set; }
        public List<ShelterState>? ShelterState { get; set; }
        public int? AbrisStatCount { get; set; }
        public string libelleResult { get; set; }
        public List<string> HistorySearch { get; set; }

        public IndexModel(ILogger<IndexModel> logger, AbrisServices abrisServices, AbrisStatServices abrisStatServices, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _abrisServices = abrisServices;
            _abrisStatServices = abrisStatServices;
            _httpContextAccessor = httpContextAccessor;
            HistorySearch = new List<string>();


        }

        public async Task OnGet()
        {
            
            Abris = await _abrisServices.GetAllAbris();
            if (Abris != null)
            {

                ShelterState = await _abrisServices.GetAllShelterState();

                foreach (var abris in Abris)
                {
                    var shelter = ShelterState.Last(x => x.IdAbris == abris.RecordId);
                    abris.NbPlaces = shelter?.Available;
                }
            }


        }

        public async Task OnPostAsync()
        {
            // Récupérer la valeur de HistorySearch depuis la session
            if (_httpContextAccessor.HttpContext.Session.TryGetValue("HistorySearch", out var historySearchDataBytes))
            {
                var historySearchDataString = Encoding.UTF8.GetString(historySearchDataBytes);
                HistorySearch = JsonConvert.DeserializeObject<List<string>>(historySearchDataString);
            }
            else
            {
                HistorySearch = new List<string>();
            }
            var selected = Request.Form["SelectedAbri"];
            var dateStart = Request.Form["DateStart"];
            var dateEnd = Request.Form["DateEnd"];

            try
            {
                Abris = await _abrisServices.GetAllAbris();
                ShelterState = await _abrisServices.GetAllShelterState();
                AbrisStatCount = await _abrisStatServices.GetAbrisStat(selected!, dateStart!, dateEnd!);

                if (AbrisStatCount > 0)
                {
                    Abris? libelleAbris = Abris.Find(x => x.RecordId == selected[0]);
                    var dateStartFormatted = DateTime.Parse(dateStart).ToString("dd MMMM yyyy 'à' HH:mm");
                    var dateEndFormatted = DateTime.Parse(dateEnd).ToString("dd MMMM yyyy 'à' HH:mm");
                    libelleResult = $"Statistiques d'intéractions utilisateur pour l'abri {libelleAbris?.Nom} du {dateStartFormatted} au {dateEndFormatted} : {AbrisStatCount}";
                    HistorySearch.Add(libelleResult);
                    _httpContextAccessor.HttpContext.Session.SetString("HistorySearch", JsonConvert.SerializeObject(HistorySearch));
                }
                else if (AbrisStatCount == 0)
                {
                    libelleResult = "Aucune statistiques enregistrée pendant cette période";
                    HistorySearch.Add(libelleResult);
                    _httpContextAccessor.HttpContext.Session.SetString("HistorySearch", JsonConvert.SerializeObject(HistorySearch));
                }

                if (Abris != null)
                {
                    foreach (var abris in Abris)
                    {
                        var shelter = ShelterState.LastOrDefault(x => x.IdAbris == abris.RecordId);
                        abris.NbPlaces = shelter?.Available;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                libelleResult = "Le formulaire n'est pas complet";
                HistorySearch.Add(libelleResult);
            }

        }
    }
}
