using IdentityServer.Constants;
using IdentityServer.Data.Identity;

using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Configuration
{
	/// <summary>
	/// Class that provides the functionality to add new roles and users in the database
	/// </summary>
	public static class UserInitialiser
	{
		#region Public Methods

		/// <summary>
		/// Adds roles in the database if they don't exists
		/// </summary>
		/// <param name="roleManager"></param>
		public static void SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.Roles.Any(r => r.Name == Roles.Administrator))
			{
				roleManager.CreateAsync(new IdentityRole { Name = Roles.Administrator, NormalizedName = Roles.Administrator })
					.Wait();
			}

			if (!roleManager.Roles.Any(r => r.Name == Roles.Tutor))
			{
				roleManager.CreateAsync(new IdentityRole { Name = Roles.Tutor, NormalizedName = Roles.Tutor })
					.Wait();
			}

			if (!roleManager.Roles.Any(r => r.Name == Roles.Student))
			{
				roleManager.CreateAsync(new IdentityRole { Name = Roles.Student, NormalizedName = Roles.Student })
					.Wait();
			}
		}

		/// <summary>
		/// Adds users in the database if they don't exists
		/// </summary>
		/// <param name="userManager"></param>
		public static void SeedUsers(UserManager<AppUser> userManager)
		{
			var tomUser = userManager.FindByEmailAsync
				("ts322@student.london.ac.uk").Result;

			if (tomUser == null)
			{
				var user = new AppUser
				{
					UserName = "ts322@student.london.ac.uk",
					FirstName = "Trupti",
					LastName = "Singh",
					DateOfBirth = new DateTime(1990, 01, 01),
					Email = "ts322@student.london.ac.uk",
					GenderId = 1,
					Address1 = "Address 1",
					Address2 = "Address 2",
					Address3 = "Address 3",
					City = "Birmingham",
					County = "West Midlands",
					Country = "UK",
					EmailConfirmed = true,
					RegistrationTimestamp = DateTime.UtcNow
				};

				var result = userManager.CreateAsync(user, "P@55w0rd")
					.Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(user, Roles.Administrator)
						.Wait();
				}
			}
		}

		#endregion
	}
}
