using BizLandWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BizLandWebApp.DataContext
{
    public class BizLandDbContext:IdentityDbContext<AppUser>
    {
        public BizLandDbContext(DbContextOptions<BizLandDbContext> options):base(options)
        {

        }
       
        public DbSet<Team> Teams { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
