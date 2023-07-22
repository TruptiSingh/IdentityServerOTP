using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Data.Identity
{
	public class AppUser : IdentityUser
	{
		// Add additional profile data for application users by adding properties to this class
		[StringLength(50)]
		public string FirstName { get; set; }

		[StringLength(50)]
		public string LastName { get; set; }

		[StringLength(200)]
		public string Address1 { get; set; }

		[StringLength(200)]
		public string Address2 { get; set; }

		[StringLength(200)]
		public string Address3 { get; set; }

		[StringLength(100)]
		public string City { get; set; }

		[StringLength(100)]
		public string County { get; set; }

		[StringLength(100)]
		public string Country { get; set; }

		public DateTime DateOfBirth { get; set; }

		public int GenderId { get; set; }

		public Gender Gender { get; set; }

		public DateTime? RegistrationTimestamp { get; set; }
	}
}
