using IdentityServer.DTOs.Users;

namespace IdentityServer.Interfaces.UserServices
{
	/// <summary>
	/// Service contract that provides the facility to toggle user's active status
	/// </summary>
	public interface ISetUserActiveStatusService
	{
		/// <summary>
		/// Sets user's status as active or inactive
		/// </summary>
		///<param name="toggleUserActiveStatusDTO" <see cref="ToggleUserActiveStatusDTO"/>></param>
		Task SetUserActiveStatusAsync(ToggleUserActiveStatusDTO toggleUserActiveStatusDTO);
	}
}
