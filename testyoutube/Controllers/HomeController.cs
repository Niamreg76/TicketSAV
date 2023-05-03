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
using testyoutube.Services;

namespace testyoutube.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<testyoutubeUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly TicketDataContext _context;
        private readonly IEmailSender _emailSender;
        //private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(ILogger<HomeController> logger, TicketDataContext context, UserManager<testyoutubeUser> userManager, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            this._emailSender = emailSender;
            //_roleManager = roleManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
           
            var listTicket = _context.Tickets.Include(t => t.Statut).ToList();
            var listTicket2 = _context.Tickets.Include(t => t.panne).ToList();
            //ViewBag.ID_panne = new SelectList(_context.panne, "ID_panne", "Description");
            // ViewBag.ID_panne = new SelectList(_context.Statut, "ID_panne", "Description");

            //var email = "romangermain1@gmail.com";
            //var subject = "Test";
            //var message = "Hello World";

            //await _emailSender.SendMailAsync(email, subject, message);
            //var user = await _userManager.FindByIdAsync(userId);
            //
            //if (User != null)
            //{
            //    var roleExists = await _roleManager.RoleExistsAsync("admin");
            //
            //    if (!roleExists)
            //    {
            //        await _roleManager.CreateAsync(new IdentityRole("admin"));
            //    }
            //
            //    await _userManager.AddToRoleAsync(user, "admin");
            //}




            return View(listTicket);
        }

        public async Task<IActionResult> AddUserToRoleAsync(string userId, string rolename)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.AddToRoleAsync(user, rolename);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError("", "Impossible d'ajouter l'utilisateur au rôle");
            return View();
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

        public IActionResult ChangeStatus(int ticketId, int selectedStatus)
        {
            var ticket = _context.Tickets.Find(ticketId);
            if (ticket != null)
            {
                ticket.ID_statut = selectedStatus;
                ticket.Date_modif = DateTime.Now;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
