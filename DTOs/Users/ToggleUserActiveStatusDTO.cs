namespace IdentityServer.DTOs.Users
{
	public class ToggleUserActiveStatusDTO
	{
		public string UserId { get; set; }

		public bool IsActive { get; set; }
	}
}
