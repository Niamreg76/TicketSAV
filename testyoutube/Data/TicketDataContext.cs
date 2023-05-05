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
        public DbSet<Panne> panne { get; set;}
        public DbSet<Statut> statut { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tickets>()
                .HasOne(t => t.panne)
                .WithMany()
                .HasForeignKey(t => t.ID_panne);

            modelBuilder.Entity<Tickets>()
                .HasOne(t => t.Statut)
                .WithMany()
                .HasForeignKey(t => t.ID_statut);
        }
    }
}
