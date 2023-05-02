using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testyoutube.Data;

namespace testyoutube.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketDataContext _context;
        public IActionResult Index()
        {
            var tickets = _context.Tickets.Include(t => t.Statut).ToList();
            ViewBag.Statuses = _context.statut.ToList();
            return View(tickets);
        }
        public TicketController(TicketDataContext context)
        {
            _context = context;

        }
        public IActionResult ChangeStatus(int ticketId, int newStatus)
        {
            var ticket = _context.Tickets.Find(ticketId);
            if (ticket != null)
            {
                ticket.ID_statut = newStatus;
                ticket.Date_modif = DateTime.Now;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
