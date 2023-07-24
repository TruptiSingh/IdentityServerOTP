using IdentityServer.DTOs.Users;

namespace IdentityServer.Interfaces.UserServices
{
	/// <summary>
	/// An iterface that provides the functionality to retrieve all users from the repository
	/// </summary>
	public interface IGetAllUsersService
	{
		/// <summary>
		/// Gets all users from the repository
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<UserAdminDTO>> GetAllUsers();

		/// <summary>
		/// Retrieves all users having specified roles
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		List<UserDTO> GetUsersByRoles(List<string> roles);

		/// <summary>
		/// Retrieves user by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		UserDTO GetUserById(string id);
	}
}
