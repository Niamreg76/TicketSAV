using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using testyoutube.Data;
using testyoutube.Models;
using Microsoft.AspNetCore.Identity;
using testyoutube.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace testyoutube.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<testyoutubeUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly TicketDataContext _context;

        public HomeController(ILogger<HomeController> logger, TicketDataContext context, UserManager<testyoutubeUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
           
            var listTicket = _context.Tickets.Include(t => t.Statut).ToList();
            var listTicket2 = _context.Tickets.Include(t => t.panne).ToList();
            //ViewBag.ID_panne = new SelectList(_context.panne, "ID_panne", "Description");
            // ViewBag.ID_panne = new SelectList(_context.Statut, "ID_panne", "Description");
            return View(listTicket);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Creation()
        {
            var pannes = _context.panne.ToList();

          
            var PanneSelectList = pannes.Select(s => new SelectListItem
            {
                Value = s.ID_panne.ToString(),
                Text = s.Description
            }).ToList();
     
            PanneSelectList.Insert(0, new SelectListItem { Value = "", Text = "" });

            ViewBag.ID_panne = PanneSelectList;

            return View();
        }

        public async Task<IActionResult> CreationButton(Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.Date_creation = DateTime.Now;
                tickets.Date_modif = DateTime.Now;

                var statut = _context.statut.FirstOrDefault(s => s.ID_statut == 1);
                if (statut != null)
                {
                    tickets.Statut = statut;
                    tickets.Statut.Nom = "Ouvert";
                }

                var user = await _userManager.GetUserAsync(User);

                tickets.ID_utilisateur = user.Id;

                _context.Tickets.Add(tickets);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(tickets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
