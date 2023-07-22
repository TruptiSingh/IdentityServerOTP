using System.Threading.Tasks;

using IdentityServer.ViewModels.Account;

namespace IdentityServer.Interfaces.AccountServices
{
	/// <summary>
	/// An iterface that provides the functionality to build a view model that can be used to build a login page
	/// </summary>
	public interface IBuildLoginViewModelService
	{
		/// <summary>
		/// Builds a view model that can be used to build a login page
		/// </summary>
		/// <param name="returnUrl">User will be redirected to this URL after authentication is successful</param>
		/// <returns></returns>
		public Task<LoginViewModel> BuildLoginViewModel( string returnUrl );

		/// <summary>
		/// Builds a view model that can be used to build a login page
		/// </summary>
		/// <param name="model">Contains data user entered on the login page</param>
		/// <returns></returns>
		public Task<LoginViewModel> BuildLoginViewModel( LoginInputModel model );
	}
}
