using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using testyoutube.Data;

namespace testyoutube.Hubs
{
    public class ChatHub : Hub
    {
        private readonly TicketDataContext _context;

        public ChatHub(TicketDataContext context)
        {
            _context = context;
        }
        public async Task SendMessage( string message)
        {
            string user = Context.User.Identity.Name;
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
