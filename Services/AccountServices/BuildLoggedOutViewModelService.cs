using System.Security.Claims;

using IdentityModel;

using IdentityServer.Configuration;
using IdentityServer.Interfaces.AccountServices;
using IdentityServer.ViewModels.Account;

using IdentityServer4.Extensions;
using IdentityServer4.Services;

namespace IdentityServer.Services.AccountServices
{
	/// <summary>
	/// Implementation of <see cref="IBuildLoggedOutViewModelService"/>
	/// </summary>
	public class BuildLoggedOutViewModelService : IBuildLoggedOutViewModelService
	{
		#region Private Variables

		private readonly IIdentityServerInteractionService _interaction;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="interaction"></param>
		public BuildLoggedOutViewModelService(IIdentityServerInteractionService interaction)
		{
			_interaction = interaction;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Builds a view model that shows aprropriate URLs on the logged out page
		/// </summary>
		/// <param name="user"></param>
		/// <param name="httpContext"></param>
		/// <param name="logoutId"></param>
		/// <returns></returns>
		public async Task<LoggedOutViewModel> BuildLoggedOutViewModel(ClaimsPrincipal user, HttpContext httpContext, string logoutId)
		{
			// get context information (client name, post logout redirect URI and iframe for federated signout)
			var logout = await _interaction.GetLogoutContextAsync(logoutId);

			var vm = new LoggedOutViewModel
			{
				AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
				PostLogoutRedirectUri = logout?.PostLogoutRedirectUri ?? "http://localhost:4200",
				ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
				SignOutIframeUrl = logout?.SignOutIFrameUrl,
				LogoutId = logoutId
			};

			if (user?.Identity.IsAuthenticated == true)
			{
				var idp = user.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

				if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
				{
					var providerSupportsSignout = await httpContext.GetSchemeSupportsSignOutAsync(idp);

					if (providerSupportsSignout)
					{
						if (vm.LogoutId == null)
						{
							// if there's no current logout context, we need to create one
							// this captures necessary info from the current logged in user
							// before we signout and redirect away to the external IdP for signout
							vm.LogoutId = await _interaction.CreateLogoutContextAsync();
						}

						vm.ExternalAuthenticationScheme = idp;
					}
				}
			}

			return vm;
		}

		#endregion
	}
}
