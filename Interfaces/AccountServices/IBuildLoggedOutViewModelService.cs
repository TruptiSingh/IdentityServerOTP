using System.Security.Claims;
using System.Threading.Tasks;

using IdentityServer.ViewModels.Account;

using Microsoft.AspNetCore.Http;

namespace IdentityServer.Interfaces.AccountServices
{
	/// <summary>
	/// An interface that provides functionality to build a view model that shows aprropriate URLs on the logged out page
	/// </summary>
	public interface IBuildLoggedOutViewModelService
	{
		/// <summary>
		/// Builds a view model that shows aprropriate URLs on the logged out page
		/// </summary>
		/// <param name="user"></param>
		/// <param name="httpContext"></param>
		/// <param name="logoutId"></param>
		/// <returns></returns>
		public Task<LoggedOutViewModel> BuildLoggedOutViewModel( ClaimsPrincipal user, HttpContext httpContext, string logoutId );
	}
}
