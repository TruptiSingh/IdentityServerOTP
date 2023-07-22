using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels.Account
{
	public class LoginInputModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Username { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }

		public bool RememberLogin { get; set; }

		public string ReturnUrl { get; set; }
	}
}
