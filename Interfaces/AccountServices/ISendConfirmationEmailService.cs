using System.Threading.Tasks;

using IdentityServer.Data.Identity;

namespace IdentityServer.Interfaces.AccountServices
{
	/// <summary>
	/// An iterface that provides the functionality to send a confirmation email to the user
	/// </summary>
	public interface ISendConfirmationEmailService
	{
		/// <summary>
		/// Sends a confirmation email
		/// </summary>
		/// <param name="user">User for which email should be confirmed</param>
		/// <param name="returnUrl">Used to create confirmation link</param>
		/// <returns></returns>
		Task SendConfirmationEmail( AppUser user, string returnUrl );
	}
}
