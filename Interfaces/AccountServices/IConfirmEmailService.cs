using IdentityServer.ViewModels.Account;

namespace IdentityServer.Interfaces.AccountServices
{
	/// <summary>
	/// An iterface that provides the functionality to confirm user's email address 
	/// </summary>
	public interface IConfirmEmailService
	{
		/// <summary>
		/// Confirms user's email address
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="token"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		EmailConfirmedViewModel ConfirmEmail( string userid, string token, string returnUrl );
	}
}
