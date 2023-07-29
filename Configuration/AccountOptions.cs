namespace IdentityServer.Configuration
{
	/// <summary>
	/// Class that provides different options for logging into an account
	/// </summary>
	public static class AccountOptions
	{
		#region Public Properties

		/// <summary>
		/// Flag indicating whetherlocal logins should be allowed or not
		/// </summary>
		public static bool AllowLocalLogin = true;

		/// <summary>
		/// Flag indicating whether login details should remembered or not
		/// </summary>
		public static bool AllowRememberLogin = true;

		/// <summary>
		/// How long login details should be remembered
		/// </summary>
		public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

		/// <summary>
		/// Flag indicating whether logout cofirmation should be shown or not
		/// </summary>
		public static bool ShowLogoutPrompt = true;

		/// <summary>
		/// Flag indicating whether user should be redirected to another Url or not
		/// </summary>
		public static bool AutomaticRedirectAfterSignOut = false;

		/// <summary>
		/// Specify the Windows authentication scheme being used
		/// </summary>
		public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;

		/// <summary>
		/// Flag indicating whether we load the groups from windows or not if user uses windows auth
		/// </summary>
		public static bool IncludeWindowsGroups = false;

		#endregion
	}
}
