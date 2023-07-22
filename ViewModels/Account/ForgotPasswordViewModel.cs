using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels.Account
{
	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Email { get; set; }

		public string ReturnUrl { get; set; }
	}
}
