using IdentityServer.Constants;
using IdentityServer.Data.Identity;
using IdentityServer.Interfaces.UserServices;

using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services.UserServices
{
	/// <summary>
	/// Implementation of <see cref="ISetUserAccessRightsService"/>
	/// </summary>
	public class SetUserAccessRightsService : ISetUserAccessRightsService
	{
		#region Private Variables

		private readonly UserManager<AppUser> _userManager;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="userManager"></param>
		public SetUserAccessRightsService(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets user access rights for the portal
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="role"></param>
		/// <param name="setAccess"></param>
		public void SetUserAccessRights(string userId, string role, bool setAccess)
		{
			try
			{
				if (role != Roles.Administrator &&
					role != Roles.Tutor &&
					role != Roles.Student)
				{
					throw new Exception("Invalid RoleName: " + role);
				}

				var user = _userManager.FindByIdAsync(userId).Result;

				if (setAccess)
				{
					_userManager.AddToRoleAsync(user, role)
							.Wait();
				}
				else
				{
					_userManager.RemoveFromRoleAsync(user, role)
						.Wait();
				}
			}
			catch (Exception)
			{
				throw new Exception("An error has occured, if this continues please contact customer An error has occured, if this continues please contact customer support");
			}
		}

		#endregion
	}
}
