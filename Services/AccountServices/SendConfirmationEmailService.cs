using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

using IdentityServer.Data.Identity;
using IdentityServer.Interfaces.AccountServices;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

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

		private readonly string _sendGridSmtpKey;

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

			_sendGridSmtpKey = configuration ["SendGridSmtpKey"];
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
				From = new MailAddress("deo.sushrut@gmail.com"),
				IsBodyHtml = true,
				Subject = "Confirm your email"
			};

			msg.To.Add(new MailAddress(user.Email));

			msg.Body = $"<html><body>Hello {user.FirstName} {user.LastName}. Please click " +
				$"<a href=\"{confirmationLink}\">here</a> to confirm your email address</body></html>";

			SmtpClient smtpClient = new()
			{
				Credentials = new NetworkCredential("apikey", _sendGridSmtpKey),
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Host = "smtp.sendgrid.net",
				Port = 587,
				EnableSsl = false
			};

			smtpClient.Send(msg);
		}

		#endregion
	}
}
