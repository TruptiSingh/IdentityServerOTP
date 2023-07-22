namespace IdentityServer.DTOs.Users
{
	public class UserDTO
	{
		public string Id { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string Address3 { get; set; }

		public string City { get; set; }

		public string County { get; set; }

		public string Country { get; set; }

		public DateTime DateOfBirth { get; set; }

		public int GenderId { get; set; }

		public string RoleId { get; set; }

		public string RoleName { get; set; }
	}
}
