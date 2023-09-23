namespace IdentityServer.DTOs.Users
{
	public class UserAdminDTO
	{
		public string Id { get; set; }

		public string Username { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public bool IsAdmin { get; set; }

		public bool IsTutor { get; set; }

		public bool IsStudent { get; set; }

		public bool IsActive { get; set; }
	}
}
