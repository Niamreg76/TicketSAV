using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using testyoutube.Areas.Identity.Data;

namespace testyoutube.Data
{
    public class UserDataContext : IdentityDbContext<testyoutubeUser>
    {
        public UserDataContext(DbContextOptions<UserDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<testyoutubeUser>
{
    public void Configure(EntityTypeBuilder<testyoutubeUser> builder)
    {

    }
}
