using FrontMDD.Entities;
using FrontMDD.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrontMDD.WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AbrisServices _abrisServices;

        public List<Abris>? Abris { get; set; }

        public IndexModel(ILogger<IndexModel> logger, AbrisServices abrisServices)
        {
            _logger = logger;
            _abrisServices = abrisServices;
        }

        public async Task OnGetAsync()
        {
            Abris = await _abrisServices.GetAllAbris();

        }
    }
}
