namespace IdentityServer.Interfaces.UserServices
{
	/// <summary>
	/// An iterface that provides the functionality to set user access rights for the portal
	/// </summary>
	public interface ISetUserAccessRightsService
	{
		/// <summary>
		/// Sets user access rights for the portal
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="role"></param>
		/// <param name="setAccess"></param>
		void SetUserAccessRights( string userId, string role, bool setAccess );
	}
}
