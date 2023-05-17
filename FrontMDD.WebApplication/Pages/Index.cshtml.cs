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


        public List<Abris>? Abris { get; set; }
        public List<ShelterState>? ShelterState { get; set; }

        public IndexModel(ILogger<IndexModel> logger, AbrisServices abrisServices)
        {
            _logger = logger;
            _abrisServices = abrisServices;

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
            // do something with emailAddress
        }

    }
}
