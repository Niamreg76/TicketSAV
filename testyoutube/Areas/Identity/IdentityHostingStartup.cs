using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using testyoutube.Areas.Identity.Data;
using testyoutube.Data;

[assembly: HostingStartup(typeof(testyoutube.Areas.Identity.IdentityHostingStartup))]
namespace testyoutube.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserDataContext>(options =>
                    options.UseMySQL(
                        context.Configuration.GetConnectionString("testyoutubeContextConnection")));

                services.AddDefaultIdentity<testyoutubeUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<UserDataContext>();
            });
        }
    }
}