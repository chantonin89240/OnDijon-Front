using FrontMDD.Entities;
using FrontMDD.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontMDD.WebApplication.Pages
{
    public class StatistiquesModel : PageModel
    {
        private readonly ILogger<StatistiquesModel> _logger;
        private readonly ProfilServices _profilService;

        public StatistiquesModel(ILogger<StatistiquesModel> logger, ProfilServices profilServices)
        {
            _logger = logger;
            _profilService = profilServices;
        }

        public List<Profil> ListProfils { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ListProfils = await _profilService.GetAllProfil();
                return Page();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}