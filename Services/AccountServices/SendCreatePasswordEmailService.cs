using IdentityServer.Data.Identity;
using IdentityServer.Interfaces.AccountServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Web;

namespace IdentityServer.Services.AccountServices
{
	/// <summary>
	/// Implementation of <see cref="ISendCreatePasswordEmailService"/>
	/// </summary>
	public class SendCreatePasswordEmailService : ISendCreatePasswordEmailService
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
		public SendCreatePasswordEmailService(UserManager<AppUser> userManager,
			IConfiguration configuration)
		{
			_userManager = userManager;

			_identityServerSite = configuration ["IdentityServerSite"];
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sends a password creation email
		/// </summary>
		/// <param name="emailAddress">Email address of the user</param>
		/// <param name="returnUrl">Url to which user should be redirected</param>
		/// <returns></returns>
		public async Task SendCreatePasswordEmail(string emailAddress, string returnUrl)
		{
			var user = await _userManager.FindByNameAsync(emailAddress);

			if (user != null)
			{
				// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
				// Send an email with this link
				var code = await _userManager.GeneratePasswordResetTokenAsync(user);
				var codeGeneratedBytes = Encoding.UTF8.GetBytes(code);
				var codeEncoded = WebEncoders.Base64UrlEncode(codeGeneratedBytes);

				var resetLink = string.Concat(_identityServerSite, "Account/CreatePassword?userid=",
					user.Id, "&code=", codeEncoded, "&returnUrl=", HttpUtility.UrlEncode(returnUrl));

				//var msg = new SendGridMessage();
				//msg.SetFrom( "notifications@nonacus.com", "Nonacus" );
				//msg.SetTemplateId( "d-e0bd61cb1a4d4622bb831bba740a1255" );

				//msg.SetTemplateData( new
				//{
				//	subject = "Nonacus Portal Invitation",
				//	heading = "Hello " + string.Concat( user.FirstName, ' ', user.Surname ) + ",",
				//	body = "You have been invited to the Nonacus Portal",
				//	link_text = "Please click here to create your password",
				//	link_url = resetLink
				//} );

				//msg.AddTo( new EmailAddress( user.Email ) );

				//await _sendEmailService.SendEmail( msg );
			}
		}

		#endregion
	}
}
