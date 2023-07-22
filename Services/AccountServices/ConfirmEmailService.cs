using IdentityServer.Data.Identity;
using IdentityServer.Interfaces.AccountServices;
using IdentityServer.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace IdentityServer.Services.AccountServices
{
	/// <summary>
	/// Implementation of <see cref="IConfirmEmailService"/>
	/// </summary>
	public class ConfirmEmailService : IConfirmEmailService
	{
		#region Private Variables

		private readonly UserManager<AppUser> _userManager;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="userManager"></param>
		public ConfirmEmailService(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Confirms user's email address 
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="token"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		public EmailConfirmedViewModel ConfirmEmail(string userid, string token, string returnUrl)
		{
			var user = _userManager.FindByIdAsync(userid).Result;

			var codeDecodedBytes = WebEncoders.Base64UrlDecode(token);

			var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

			var result = _userManager.ConfirmEmailAsync(user, codeDecoded).Result;

			return new EmailConfirmedViewModel
			{
				ConfirmationMessage = result.Succeeded ? "Email confirmed successfully" : "Error while confirming your email, if this continues please contact customer support",
				RedirectUri = returnUrl
			};
		}

		#endregion
	}
}
