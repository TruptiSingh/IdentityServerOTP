using System.Security.Claims;
using System.Threading.Tasks;

using IdentityServer.ViewModels.Account;

namespace IdentityServer.Interfaces.AccountServices
{
	/// <summary>
	/// An iterface that provides the functionality to build a view model that can be used to build a logout page
	/// </summary>
	public interface IBuildLogoutViewModelService
	{
		/// <summary>
		/// Builds a view model that can be used to build a logout page
		/// </summary>
		/// <param name="user"></param>
		/// <param name="logoutId"></param>
		/// <returns></returns>
		public Task<LogoutViewModel> BuildLogoutViewModel( ClaimsPrincipal user, string logoutId );
	}
}
