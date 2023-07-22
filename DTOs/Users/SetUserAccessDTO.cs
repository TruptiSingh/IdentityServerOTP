namespace IdentityServer.DTOs.Users
{
	public class SetUserAccessDTO
	{
		public string UserId { get; set; }

		public string RoleName { get; set; }

		public bool SetAccess { get; set; }
	}
}
