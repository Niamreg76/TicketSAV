﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly UserManager<aspnetusers> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly TicketDataContext _context;
        private readonly IEmailSender _emailSender;
        //private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(ILogger<HomeController> logger, TicketDataContext context, UserManager<aspnetusers> userManager, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            this._emailSender = emailSender;
            //_roleManager = roleManager;
        }

        public IActionResult Index()
        {
            //affichage de la première page
            var listTicket = _context.Tickets
                .Include(t => t.Statut) 
                .Include(t => t.Panne)
                .ToList();

            return View(listTicket);
        }

        public IActionResult Privacy()                            
        {                                                         
            return View();                                        
        }                                                         
                                                                  
        public IActionResult Creation()                           
        {                                                         
            var pannes = _context.Panne.ToList();

            var categories = _context.CategPanne.ToList();

            var CategPanneSelectList = categories.Select(s => new SelectListItem
            {
                Value = s.ID_CategPanne.ToString(),
                Text = s.Nom
            }).ToList();
          
            var PanneSelectList = pannes.Select(s => new SelectListItem
            {
                Value = s.ID_panne.ToString(),
                Text = s.Description
            }).ToList();
     
            PanneSelectList.Insert(0, new SelectListItem { Value = "", Text = "" });
            CategPanneSelectList.Insert(0, new SelectListItem { Value = "", Text = "" });

            ViewBag.ID_panne = PanneSelectList;
            ViewBag.Categ = CategPanneSelectList;

            return View();
        }

        public async Task<IActionResult> CreationButton(Tickets tickets, Conversation conversation)
        {
            if (ModelState.IsValid)
            {
                tickets.Date_creation = DateTime.Now;
                tickets.Date_modif = DateTime.Now;
                tickets.ID_conversation = tickets.ID_ticket;
                //ici on attribue les dates ainsi que l'id du formulaire aux données de la base de données

                var statut = _context.Statut.FirstOrDefault(s => s.ID_statut == 1);
                if (statut != null)
                {
                    tickets.Statut = statut;
                    tickets.Statut.Nom = "Ouvert";
                    //ici on attribue directement le statut du ticket en "ouvert"
                }

                var user = await _userManager.GetUserAsync(User);
                tickets.ID_utilisateur = user.Id;
                //ici on récupère l'id de l'utilisateur depuis Identity

                _context.Tickets.Add(tickets);
                await _context.SaveChangesAsync();
                //on ajoute le ticket à la base de données

                tickets.ID_conversation = tickets.ID_ticket;
                await _context.SaveChangesAsync();

                if (ModelState.IsValid)
                {
                    conversation.ID_conversation = tickets.ID_conversation;
                    conversation.ID_ticket = tickets.ID_ticket;

                    _context.Conversations.Add(conversation);
                    await _context.SaveChangesAsync();
                }

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
        public IActionResult GetPannesByCategoryId(int categoryId)
        {
            var pannes = _context.Panne.Where(p => p.ID_categPanne == categoryId).ToList();

            var panneSelectList = pannes.Select(p => new SelectListItem
            {
                Value = p.ID_panne.ToString(),
                Text = p.Description
            }).ToList();

            return Json(panneSelectList);
        }
    }


}
