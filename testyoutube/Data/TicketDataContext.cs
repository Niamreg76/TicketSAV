using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testyoutube.Models;

namespace testyoutube.Data
{
    public class TicketDataContext : DbContext
    {
        public TicketDataContext(DbContextOptions<TicketDataContext> options) : base(options)
        {

        }

        public DbSet<Tickets> Tickets { get; set; }
        //erreur ici lors de la génération d'un controlleur pour le ticket' regarder la vidéo 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Tickets>().OwnsOne(x => x.ID_ticket);
        }
    }
}
