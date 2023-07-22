using IdentityServer.Configuration;
using IdentityServer.Interfaces.AccountServices;
using IdentityServer.ViewModels.Account;

using IdentityServer4.Services;
using IdentityServer4.Stores;

using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Services.AccountServices
{
	/// <summary>
	/// Implementation of <see cref="IBuildLoginViewModelService"/>
	/// </summary>
	public class BuildLoginViewModelService : IBuildLoginViewModelService
	{
		#region Private Variables

		private readonly IIdentityServerInteractionService _interaction;
		private readonly IAuthenticationSchemeProvider _schemeProvider;
		private readonly IClientStore _clientStore;

		private readonly string _portalSite;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="interaction"></param>
		/// <param name="schemeProvider"></param>
		/// <param name="clientStore"></param>
		/// <param name="configuration"></param>
		public BuildLoginViewModelService(IIdentityServerInteractionService interaction,
			IAuthenticationSchemeProvider schemeProvider,
			IClientStore clientStore,
			IConfiguration configuration)
		{
			_interaction = interaction;
			_schemeProvider = schemeProvider;
			_clientStore = clientStore;

			_portalSite = configuration ["PortalSite"];
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Builds a view model that can be used to build a login page.
		/// </summary>
		/// <param name="returnUrl">User will be redirected to this URL after authentication is successful</param>
		/// <returns></returns>
		public async Task<LoginViewModel> BuildLoginViewModel(string returnUrl)
		{
			var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

			var enableRegistration = false;
			var allowRememberLogin = AccountOptions.AllowRememberLogin;

			if (!string.IsNullOrEmpty(context?.RedirectUri) &&
				context.RedirectUri.StartsWith(_portalSite))
			{
				enableRegistration = true;
			}

			if (!string.IsNullOrEmpty(context?.RedirectUri))
			{
				allowRememberLogin = false;
			}

			if (context?.IdP != null)
			{
				var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

				// this is meant to short circuit the UI and only trigger the one external IdP
				var vm = new LoginViewModel
				{
					EnableLocalLogin = local,
					ReturnUrl = returnUrl,
					Username = context?.LoginHint,
					EnableRegistration = enableRegistration,
					ApplicationName = ""
				};

				return vm;
			}

			await _schemeProvider.GetAllSchemesAsync();

			var allowLocal = true;

			if (context?.Client.ClientId != null)
			{
				var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

				if (client != null)
				{
					allowLocal = client.EnableLocalLogin;
				}
			}

			return new LoginViewModel
			{
				AllowRememberLogin = allowRememberLogin,
				EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
				ReturnUrl = returnUrl,
				Username = context?.LoginHint,
				EnableRegistration = enableRegistration,
				ApplicationName = ""
			};
		}

		/// <summary>
		/// Builds a view model that can be used to build a login page.
		/// </summary>
		/// <param name="model">Contains data user entered on the login page</param>
		/// <returns></returns>
		public async Task<LoginViewModel> BuildLoginViewModel(LoginInputModel model)
		{
			var vm = await BuildLoginViewModel(model.ReturnUrl);

			vm.Username = model.Username;
			vm.RememberLogin = model.RememberLogin;

			return vm;
		}

		#endregion
	}
}
