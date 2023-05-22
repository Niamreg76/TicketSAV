using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testyoutube.Areas.Identity.Data;
using testyoutube.Models;

namespace testyoutube.Data
{
    public class TicketDataContext : DbContext
    {
        public TicketDataContext(DbContextOptions<TicketDataContext> options) : base(options)
        {

        }

        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Panne> Panne { get; set;}
        public DbSet<Statut> Statut { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<aspnetusers> aspnetusers { get; set; }
        public DbSet<CategPanne> CategPanne { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tickets>()
                .HasOne(t => t.Panne)
                .WithMany()
                .HasForeignKey(t => t.ID_panne);

            modelBuilder.Entity<Tickets>()
                .HasOne(t => t.Statut)
                .WithMany()
                .HasForeignKey(t => t.ID_statut);

        }
    }
}
