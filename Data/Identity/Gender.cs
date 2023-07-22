using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Data.Identity
{
	public class Gender
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public ICollection<AppUser> AppUsers { get; set; }
	}
}
