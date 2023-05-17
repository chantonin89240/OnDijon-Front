using FrontMDD.Entities;
using FrontMDD.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;

namespace FrontMDD.WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AbrisServices _abrisServices;
        private readonly AbrisStatServices _abrisStatServices;


        public List<Abris>? Abris { get; set; }
        public List<ShelterState>? ShelterState { get; set; }
        public int? AbrisStatCount { get; set; }
        public string libelleResult { get; set; }

        public IndexModel(ILogger<IndexModel> logger, AbrisServices abrisServices, AbrisStatServices abrisStatServices)
        {
            _logger = logger;
            _abrisServices = abrisServices;
            _abrisStatServices = abrisStatServices;

        }

        public async Task OnGetAsync()
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
            var selected = Request.Form["SelectedAbri"];
            var dateStart = Request.Form["DateStart"];
            var dateEnd = Request.Form["DateEnd"];
            var test = selected[0];
            try
            {

                Abris = await _abrisServices.GetAllAbris();
                ShelterState = await _abrisServices.GetAllShelterState();
                AbrisStatCount = await _abrisStatServices.GetAbrisStat(selected!, dateStart!, dateEnd!);


                if (AbrisStatCount > 0) 
                {
                    Abris? libelleAbris = Abris.Find(x => x.RecordId == selected[0]);
                    libelleResult = "Statistiques d'intéractions utilisateur pour l'abri " + libelleAbris?.Nom + " : " + AbrisStatCount;
                } else if(AbrisStatCount == 0)
                {
                    libelleResult = "Aucune statistiques enregistrer pendant cette période";
                    
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
                // Gérer l'erreur ici, par exemple en définissant un message d'erreur à afficher à l'utilisateur
                ViewData["ErrorMessage"] = "Une erreur s'est produite lors du calcul des statistiques des abris.";
            }
        }


    }
}
