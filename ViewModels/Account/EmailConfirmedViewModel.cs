namespace IdentityServer.ViewModels.Account
{
	public class EmailConfirmedViewModel
	{
		public string RedirectUri { get; set; }

		public string SignOutIframeUrl { get; set; }

		public string ConfirmationMessage { get; set; }
	}
}
