using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DoNotTrustWebClient.Models;

namespace DoNotTrustWebClient.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

		public DbSet<DoNotTrustWebClient.Models.ApplicationUser> ApplicationUser { get; set; }
		public DbSet<DoNotTrustWebClient.Models.Department> Department { get; set; }
		public DbSet<DoNotTrustWebClient.Models.Company> Company { get; set; }

		internal void EnsureSeedData()
		{
			if (!this.Set<Department>().Any())
			{
				var havit = new Company { Name = "HAVIT" };
				var moonfish = new Company { Name = "MoonFish" };
				var havitBlueTeam = new Department { Name = "Blue Team", Company = havit };
				var havitYellowTeam = new Department { Name = "Yellow Team", Company = havit };
				var havitGreenTeam = new Department { Name = "Green Team", Company = havit };
				var moonfishTeam = new Department { Name = "All", Company = moonfish };

				this.Add(havitBlueTeam);
				this.Add(havitYellowTeam);
				this.Add(havitGreenTeam);
				this.Add(moonfishTeam);
				SaveChanges();
			}
		}
	}
}
