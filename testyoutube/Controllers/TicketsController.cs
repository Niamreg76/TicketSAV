using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testyoutube.Data;

namespace testyoutube.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketDataContext _context;

        public TicketsController(TicketDataContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var ticketDataContext = _context.Tickets.Include(t => t.Statut).Include(t => t.panne);
            return View(await ticketDataContext.ToListAsync());
        }

        public IActionResult Chat(int? id)
        {
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
                .Include(t => t.panne)
                .FirstOrDefaultAsync(m => m.ID_ticket == id);
            if (tickets == null)
            {
                return NotFound();
            }

            return View(tickets);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["ID_statut"] = new SelectList(_context.statut, "ID_statut", "ID_statut");
            ViewData["ID_panne"] = new SelectList(_context.panne, "ID_panne", "ID_panne");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_ticket,ID_utilisateur,Titre,Description,ID_statut,ID_panne,Date_creation,Date_modif")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tickets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_statut"] = new SelectList(_context.statut, "ID_statut", "ID_statut", tickets.ID_statut);
            ViewData["ID_panne"] = new SelectList(_context.panne, "ID_panne", "ID_panne", tickets.ID_panne);
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
            ViewData["ID_statut"] = new SelectList(_context.statut, "ID_statut", "ID_statut", tickets.ID_statut);
            ViewData["ID_panne"] = new SelectList(_context.panne, "ID_panne", "ID_panne", tickets.ID_panne);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_statut"] = new SelectList(_context.statut, "ID_statut", "ID_statut", tickets.ID_statut);
            ViewData["ID_panne"] = new SelectList(_context.panne, "ID_panne", "ID_panne", tickets.ID_panne);
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
                .Include(t => t.panne)
                .FirstOrDefaultAsync(m => m.ID_ticket == id);
            if (tickets == null)
            {
                return NotFound();
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
            return RedirectToAction(nameof(Index));
        }

        private bool TicketsExists(int id)
        {
            return _context.Tickets.Any(e => e.ID_ticket == id);
        }
    }
}
