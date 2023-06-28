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
        public ShelterStateIA shelterIA { get; set; }
        public int? AbrisStatCount { get; set; }
        public string? libelleResult { get; set; }
        public List<string> HistorySearch { get; set; }

        public IndexModel(ILogger<IndexModel> logger, AbrisServices abrisServices, AbrisStatServices abrisStatServices, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _abrisServices = abrisServices;
            _abrisStatServices = abrisStatServices;
            _httpContextAccessor = httpContextAccessor;
            HistorySearch = new List<string>();


        }

        public async Task OnGetAsync()
        {

            Abris = await _abrisServices.GetAllAbris();
            if (Abris != null)
            {

                ShelterState = await _abrisServices.GetAllShelterState();

                foreach (var abris in Abris)
                {
                    int? totalVelo = await _abrisStatServices.GetShelterStatIA(abris.RecordId);
                    abris.TotalVelo = totalVelo;

                    var shelter = ShelterState.LastOrDefault(x => x.IdAbris == abris.RecordId);
                    abris.NbPlaces = shelter?.Available;
                }
            }


        }

        public async Task OnPostAsync()
        {
            var selected = Request.Form["SelectedAbri"];
            var dateStart = Request.Form["DateStart"];
            var dateEnd = Request.Form["DateEnd"];

            // Récupérer la valeur de HistorySearch depuis la session
            if (_httpContextAccessor.HttpContext!.Session.TryGetValue("HistorySearch", out var historySearchDataBytes))
            {
                var historySearchDataString = Encoding.UTF8.GetString(historySearchDataBytes);
                HistorySearch = JsonConvert.DeserializeObject<List<string>>(historySearchDataString);
                if (HistorySearch?.Count >= 5)
                {
                    HistorySearch = new List<string>();
                }
            }
            else
            {
                HistorySearch = new List<string>();
            }

            try
            {

                Abris = await _abrisServices.GetAllAbris();
                ShelterState = await _abrisServices.GetAllShelterState();
                
                if (Abris != null)
                {
                    foreach (var abris in Abris)
                    {
                        var shelter = ShelterState.LastOrDefault(x => x.IdAbris == abris.RecordId);
                        abris.NbPlaces = shelter?.Available;
                    }
                }

                AbrisStatCount = await _abrisStatServices.GetAbrisStat(selected!, dateStart!, dateEnd!);


                if (AbrisStatCount > 0)
                {
                    Abris? libelleAbris = Abris.Find(x => x.RecordId == selected[0]);
                    var dateStartFormated = DateTime.Parse(dateStart!);
                    var dateEndFormated = DateTime.Parse(dateEnd!);
                    libelleResult = "Statistiques d'intéractions utilisateur pour l'abri " + libelleAbris?.Nom + " Du " + dateStartFormated + " Au " + dateEndFormated + " : " + AbrisStatCount;
                    HistorySearch?.Add(libelleResult);
                    _httpContextAccessor.HttpContext.Session.SetString("HistorySearch", JsonConvert.SerializeObject(HistorySearch));

                }
                else if (AbrisStatCount == 0)
                {
                    libelleResult = "Aucune statistiques enregistrer pendant cette période";
                    HistorySearch?.Add(libelleResult);
                    _httpContextAccessor.HttpContext.Session.SetString("HistorySearch", JsonConvert.SerializeObject(HistorySearch));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                libelleResult = "Le formulaire n'est pas complet";
                HistorySearch?.Add(libelleResult);
                _httpContextAccessor.HttpContext.Session.SetString("HistorySearch", JsonConvert.SerializeObject(HistorySearch));
                // Gérer l'erreur ici, par exemple en définissant un message d'erreur à afficher à l'utilisateur
                ViewData["ErrorMessage"] = "Une erreur s'est produite lors du calcul des statistiques des abris.";
            }
        }


    }
}
