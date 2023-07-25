using IdentityServer.Data.Identity;
using IdentityServer.DTOs.Users;
using IdentityServer.Interfaces.UserServices;

using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services.UserSerices
{
	/// <summary>
	/// Implementation of <see cref="ISetUserActiveStatusService"/>
	/// </summary>
	public class SetUserActiveStatusService : ISetUserActiveStatusService
	{
		#region Private Variables

		private readonly UserManager<AppUser> _userManager;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="userManager"></param>
		public SetUserActiveStatusService(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets user's status as active or inactive
		/// </summary>
		///<param name="toggleUserActiveStatusDTO" <see cref="ToggleUserActiveStatusDTO"/>></param>
		public async Task SetUserActiveStatusAsync(ToggleUserActiveStatusDTO toggleUserActiveStatusDTO)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(toggleUserActiveStatusDTO.UserId);

				if (user == null)
				{
					throw new Exception("An error has occured, if this continues please contact customer An error has occured, if this continues please contact customer support");
				}

				if (toggleUserActiveStatusDTO.IsActive)
				{
					user.LockoutEnd = null;
				}
				else
				{
					user.LockoutEnd = new DateTimeOffset(new DateTime(3000, 1, 1));
				}

				await _userManager.UpdateAsync(user);
			}
			catch (Exception)
			{
				throw new Exception("An error has occured, if this continues please contact customer An error has occured, if this continues please contact customer support");
			}
		}

		#endregion
	}
}
