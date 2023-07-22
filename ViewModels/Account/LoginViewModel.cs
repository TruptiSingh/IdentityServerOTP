namespace IdentityServer.ViewModels.Account
{
	public class LoginViewModel : LoginInputModel
	{
		public bool AllowRememberLogin { get; set; } = true;

		public bool EnableLocalLogin { get; set; } = true;

		public bool EnableRegistration { get; set; } = true;

		public string ApplicationName { get; set; }
	}
}
