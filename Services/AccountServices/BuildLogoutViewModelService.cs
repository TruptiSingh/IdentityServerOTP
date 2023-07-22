using IdentityServer.Configuration;
using IdentityServer.Interfaces.AccountServices;
using IdentityServer.ViewModels.Account;
using IdentityServer4.Services;
using System.Security.Claims;

namespace IdentityServer.Services.AccountServices
{
	/// <summary>
	/// Implementation of <see cref="IBuildLogoutViewModelService"/>
	/// </summary>
	public class BuildLogoutViewModelService : IBuildLogoutViewModelService
	{
		#region Private Variables

		private readonly IIdentityServerInteractionService _interaction;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="interaction"></param>
		public BuildLogoutViewModelService(IIdentityServerInteractionService interaction)
		{
			_interaction = interaction;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Builds a view model that can be used to build a logout page.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="logoutId"></param>
		/// <returns></returns>
		public async Task<LogoutViewModel> BuildLogoutViewModel(ClaimsPrincipal user, string logoutId)
		{
			var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

			if (user?.Identity.IsAuthenticated != true)
			{
				// if the user is not authenticated, then just show logged out page
				vm.ShowLogoutPrompt = false;

				return vm;
			}

			var context = await _interaction.GetLogoutContextAsync(logoutId);

			if (context?.ShowSignoutPrompt == false)
			{
				// it's safe to automatically sign-out
				vm.ShowLogoutPrompt = false;

				return vm;
			}

			// show the logout prompt. this prevents attacks where the user
			// is automatically signed out by another malicious web page.
			return vm;
		}

		#endregion
	}
}
