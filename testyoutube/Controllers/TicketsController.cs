using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testyoutube.Areas.Identity.Data;
using testyoutube.Data;
using IEmailSender = testyoutube.Services.IEmailSender;

namespace testyoutube.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketDataContext _context;
        private readonly UserManager<aspnetusers> _userManager;
        private readonly IEmailSender _emailSender;
        public TicketsController(TicketDataContext context, UserManager<aspnetusers> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var ticketDataContext = _context.Tickets.Include(t => t.Statut).Include(t => t.Panne);
            return View(await ticketDataContext.ToListAsync());
        }
        // GET : Tickets/Chat/5
        public async Task<IActionResult> Chat(int? id)
        {
            ViewData["Title"] = "Discussion";

            var tickets = _context.Tickets
                .Include(t => t.Statut)
                .Include(t => t.Panne)
                .FirstOrDefault(t => t.ID_ticket == id);

            var messages = _context.Messages
                .Where(m => m.ID_conversation == id)
                .Include(m => m.User)
                .ToList();

            var panne = _context.Panne.FirstOrDefault(p => p.ID_panne == tickets.ID_panne);
            var statut = _context.Statut.FirstOrDefault(s => s.ID_statut == tickets.ID_statut);

            ViewBag.TicketID = tickets.ID_ticket;
            ViewBag.UserID = await _userManager.FindByIdAsync(tickets.ID_utilisateur);
            ViewBag.Titre = tickets.Titre;
            ViewBag.Description = tickets.Description;
            ViewBag.StatutID = statut.Nom;
            ViewBag.PanneID = panne.Description;
            ViewBag.DateCreation = tickets.Date_creation;
            ViewBag.DateModif = tickets.Date_modif;

            ViewBag.ConversationId = id;
            ViewBag.Messages = messages;
 

            var ticket = _context.Tickets.FirstOrDefault(t => t.ID_ticket == id);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // L'utilisateur actuel n'est pas authentifié, renvoyer une vue ou une redirection vers une page de connexion
                return Challenge();
            }

            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            if (!isAdmin && ticket.ID_utilisateur != user.Id)
            {
                // L'utilisateur actuel n'est pas autorisé à accéder au chat du ticket, renvoyer une vue ou une redirection vers une page d'erreur d'autorisation
                return Unauthorized();
            }

            var listTicket = _context.Tickets
                .Include(t => t.Statut)
                .Include(t => t.Panne)
                .ToList();


            return View();
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets
                .Include(t => t.Statut)
                .Include(t => t.Panne)
                .FirstOrDefaultAsync(m => m.ID_ticket == id);
            if (tickets == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            if (!isAdmin)
            {
                // L'utilisateur actuel n'est pas autorisé à accéder aux détails du ticket, renvoyer une vue ou une redirection vers une page d'erreur d'autorisation
                return Unauthorized();
            }

            return View(tickets);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["ID_statut"] = new SelectList(_context.Statut, "ID_statut", "ID_statut");
            ViewData["ID_panne"] = new SelectList(_context.Panne, "ID_panne", "ID_panne");


            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            if (!isAdmin)
            {
                // L'utilisateur actuel n'est pas autorisé à accéder à la modification du ticket, renvoyer une vue ou une redirection vers une page d'erreur d'autorisation
                return Unauthorized();
            }

            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_ticket,ID_utilisateur,Titre,Description,ID_statut,ID_panne,Date_creation,Date_modif, ID_conversation")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.ID_conversation = tickets.ID_ticket;
                _context.Add(tickets);
                await _context.SaveChangesAsync();

                tickets.ID_conversation = tickets.ID_ticket;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_statut"] = new SelectList(_context.Statut, "ID_statut", "ID_statut", tickets.ID_statut);
            ViewData["ID_panne"] = new SelectList(_context.Panne, "ID_panne", "ID_panne", tickets.ID_panne);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets.FindAsync(id);
            if (tickets == null)
            {
                return NotFound();
            }
            ViewData["ID_statut"] = new SelectList(_context.Statut, "ID_statut", "ID_statut", tickets.ID_statut);
            ViewData["ID_panne"] = new SelectList(_context.Panne, "ID_panne", "ID_panne", tickets.ID_panne);

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            if (!isAdmin)
            {
                // L'utilisateur actuel n'est pas autorisé à accéder à la modification du ticket, renvoyer une vue ou une redirection vers une page d'erreur d'autorisation
                return Unauthorized();
            }

            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_ticket,ID_utilisateur,Titre,Description,ID_statut,ID_panne,Date_creation,Date_modif")] Tickets tickets)
        {
            if (id != tickets.ID_ticket)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    tickets.Date_modif = DateTime.Now;
                    _context.Update(tickets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketsExists(tickets.ID_ticket))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["ID_statut"] = new SelectList(_context.Statut, "ID_statut", "ID_statut", tickets.ID_statut);
            ViewData["ID_panne"] = new SelectList(_context.Panne, "ID_panne", "ID_panne", tickets.ID_panne);

            return View(tickets);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets
                .Include(t => t.Statut)
                .Include(t => t.Panne)
                .FirstOrDefaultAsync(m => m.ID_ticket == id);
            if (tickets == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            if (!isAdmin)
            {
                // L'utilisateur actuel n'est pas autorisé à accéder à la suppression du ticket, renvoyer une vue ou une redirection vers une page d'erreur d'autorisation
                return Unauthorized();
            }

            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tickets = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(tickets);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool TicketsExists(int id)
        {
            return _context.Tickets.Any(e => e.ID_ticket == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChatCreate([Bind("ID_message,ID_utilisateur,Contenu,DateMessage,ID_conversation")] Message message, string messageInput, int conversationId)
        {
            if (ModelState.IsValid)
            {
                    var user = await _userManager.GetUserAsync(User);
                    message.ID_utilisateur = user.Id;
                    message.DateMessage = DateTime.Now;
                    message.ID_conversation = conversationId;
                    message.Contenu = messageInput;

                    await _context.Messages.AddAsync(message);
                    _context.Add(message);
                    await _context.SaveChangesAsync();

/*                  var user2 = await _userManager.GetUserAsync(User);
                    bool isAdmin = await _userManager.IsInRoleAsync(user2, "admin");
                    string message2 = "Smartlight SAV\n\nBonjour, \n\n Un message vient d'être envoyé dans le chat du ticket numéro : \n\n " + conversationId + " \n\n Ce message est généré automatiquement.";

                    if (!isAdmin)
                    {
                        await _emailSender.SendMailAsync("romanautomail123@gmail.com", "Nouveau message pour le ticket", message2);
                    }
                    else
                    {
                        await _emailSender.SendMailAsync(user2.Email, "Nouveau message pour le ticket", message2);
                    }*/

                            string referer = Request.Headers["Referer"].ToString();
                            return Redirect(referer);
                    }
            return View();
        }
    }
}
