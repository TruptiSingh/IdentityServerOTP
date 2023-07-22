using System.Threading.Tasks;

namespace IdentityServer.Interfaces.AccountServices
{
	/// <summary>
	/// An iterface that provides the functionality to send a password creation email to the user
	/// </summary>
	public interface ISendCreatePasswordEmailService
	{
		/// <summary>
		/// Sends a password creation email
		/// </summary>
		/// <param name="emailAddress">Email address of the user</param>
		/// <param name="returnUrl">Url to which user should be redirected</param>
		/// <returns></returns>
		Task SendCreatePasswordEmail( string emailAddress, string returnUrl );
	}
}
