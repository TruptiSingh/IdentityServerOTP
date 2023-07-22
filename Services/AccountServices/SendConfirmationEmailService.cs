using IdentityServer.Data.Identity;
using IdentityServer.Interfaces.AccountServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace IdentityServer.Services.AccountServices
{
	/// <summary>
	/// Implementation of <see cref="ISendConfirmationEmailService"/>
	/// </summary>
	public class SendConfirmationEmailService : ISendConfirmationEmailService
	{
		#region Private Variables

		private readonly UserManager<AppUser> _userManager;

		private readonly string _identityServerSite;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="sendEmailService"></param>
		/// <param name="configuration"></param>
		public SendConfirmationEmailService(UserManager<AppUser> userManager,
			IConfiguration configuration)
		{
			_userManager = userManager;

			_identityServerSite = configuration ["IdentityServerSite"];
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sends a confirmation email
		/// </summary>
		/// <param name="user"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		public async Task SendConfirmationEmail(AppUser user, string returnUrl)
		{
			var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

			var tokenGeneratedBytes = Encoding.UTF8.GetBytes(confirmationToken);

			var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

			var confirmationLink = string.Concat(_identityServerSite, "Account/ConfirmEmail?userid=", user.Id,
				"&token=", codeEncoded, "&returnUrl=", HttpUtility.UrlEncode(returnUrl));

			var msg = new MailMessage
			{
				From = new MailAddress(""),
				Subject = "Email Confirmation"
			};

			msg.To.Add(new MailAddress(""));

			msg.Body = $"Hello {user.FirstName} {user.LastName}. Please click {confirmationLink} to confirm your email address";
		}

		#endregion
	}
}
