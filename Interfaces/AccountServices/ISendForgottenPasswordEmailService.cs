using System.Threading.Tasks;

namespace IdentityServer.Interfaces.AccountServices
{
	/// <summary>
	/// An iterface that provides the functionality to send a password forgotton email to the user
	/// </summary>
	public interface ISendForgottenPasswordEmailService
	{
		/// <summary>
		/// Sends a password forgotton email
		/// </summary>
		/// <param name="emailAddress">Email address of the user</param>
		/// <param name="returnUrl">Url to which user should be redirected</param>
		/// <returns></returns>
		Task SendForgottenPasswordEmail( string emailAddress, string returnUrl );
	}
}
