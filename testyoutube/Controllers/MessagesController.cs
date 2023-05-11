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

namespace testyoutube.Controllers
{
    public class MessagesController : Controller
    {
        private readonly TicketDataContext _context;
        private readonly UserManager<aspnetusers> _userManager;
        public MessagesController(TicketDataContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var ticketDataContext = _context.Messages.Include(m => m.Conversation).Include(m => m.User);
            return View(await ticketDataContext.ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Conversation)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.ID_message == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            ViewData["ID_conversation"] = new SelectList(_context.Conversations, "ID_conversation", "ID_conversation");
            ViewData["ID_utilisateur"] = new SelectList(_context.aspnetusers, "Id", "Id");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_message,ID_utilisateur,Contenu,DateMessage,ID_conversation")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_conversation"] = new SelectList(_context.Conversations, "ID_conversation", "ID_conversation", message.ID_conversation);
            ViewData["ID_utilisateur"] = new SelectList(_context.Set<Tickets>(), "Id", "Id", message.ID_utilisateur);
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["ID_conversation"] = new SelectList(_context.Conversations, "ID_conversation", "ID_conversation", message.ID_conversation);
            ViewData["ID_utilisateur"] = new SelectList(_context.aspnetusers, "Id", "Id");
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_message,ID_utilisateur,Contenu,DateMessage,ID_conversation")] Message message)
        {
            if (id != message.ID_message)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.ID_message))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_conversation"] = new SelectList(_context.Conversations, "ID_conversation", "ID_conversation", message.ID_conversation);
            ViewData["ID_utilisateur"] = new SelectList(_context.Set<Tickets>(), "Id", "Id", message.ID_utilisateur);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Conversation)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.ID_message == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.ID_message == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChatCreate([Bind("ID_message,ID_utilisateur,Contenu,DateMessage,ID_conversation")] Message message)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    message.ID_utilisateur = user.Id;
                    message.DateMessage = DateTime.Now;
                    message.ID_conversation = 1;

                    await _context.Messages.AddAsync(message);
                    _context.Add(message);
                    await _context.SaveChangesAsync();

                    TempData["MessageSent"] = "Le message a été envoyé avec succès.";
                    return Ok();
                }
                catch (Exception)
                {
                    TempData["MessageError"] = "Une erreur s'est produite lors de l'envoi du message.";
                    // Rediriger vers une vue d'erreur ou afficher un message d'erreur approprié
                }

            }

            return View(message);
        }
    }
}
