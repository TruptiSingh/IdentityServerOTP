using IdentityModel;

using IdentityServer.Constants;
using IdentityServer.Data.Identity;
using IdentityServer.DTOs.Users;
using IdentityServer.Interfaces.UserServices;

using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services.UserServices
{
	/// <summary>
	/// Implementation of <see cref="IGetAllUsersService"/>
	/// </summary>
	public class GetAllUsersService : IGetAllUsersService
	{
		#region Private Variables

		private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
		private readonly UserManager<AppUser> _userManager;
		private readonly AppIdentityDbContext _appIdentityDbContext;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="claimsFactory"></param>
		/// <param name="appIdentityDbContext"></param>
		public GetAllUsersService(UserManager<AppUser> userManager,
			IUserClaimsPrincipalFactory<AppUser> claimsFactory, AppIdentityDbContext appIdentityDbContext)
		{
			_userManager = userManager;
			_claimsFactory = claimsFactory;
			_appIdentityDbContext = appIdentityDbContext;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets all users from the repository
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<UserAdminDTO>> GetAllUsers()
		{
			try
			{
				var allUsers = new List<UserAdminDTO>();

				foreach (var user in _userManager.Users.ToList())
				{
					var principal = await _claimsFactory.CreateAsync(user);

					allUsers.Add(new UserAdminDTO
					{
						Id = user.Id,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Username = user.UserName,
						IsAdmin = principal.Claims.Any(x => x.Type == JwtClaimTypes.Role &&
							 x.Value == Roles.Administrator),
						IsTutor = principal.Claims.Any(x => x.Type == JwtClaimTypes.Role &&
							 x.Value == Roles.Tutor),
						IsStudent = principal.Claims.Any(x => x.Type == JwtClaimTypes.Role &&
							 x.Value == Roles.Student),
						IsActive = !(user.LockoutEnd != null)
					});
				}

				return allUsers.OrderBy(x => x.Username);
			}
			catch (Exception)
			{
				throw new Exception("An error has occured, if this continues please contact customer An error has occured, if this continues please contact customer support");
			}
		}

		/// <summary>
		/// Retrieves all users having specified roles
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		public List<UserDTO> GetUsersByRoles(List<string> roles)
		{
			List<UserDTO> users = _appIdentityDbContext.UserRoles
				.Join(_appIdentityDbContext.Users, userRole => userRole.UserId, user => user.Id,
				(userRole, user) => new { userRole, user })
				.Join(_appIdentityDbContext.Roles, userRoleUser => userRoleUser.userRole.RoleId, role => role.Id,
				(userRolesUser, role) => new UserDTO
				{
					Id = userRolesUser.user.Id,
					FirstName = userRolesUser.user.FirstName,
					LastName = userRolesUser.user.LastName,
					Username = userRolesUser.user.UserName,
					Address1 = userRolesUser.user.Address1,
					Address2 = userRolesUser.user.Address2,
					Address3 = userRolesUser.user.Address3,
					City = userRolesUser.user.City,
					Country = userRolesUser.user.Country,
					County = userRolesUser.user.County,
					DateOfBirth = userRolesUser.user.DateOfBirth,
					Email = userRolesUser.user.Email,
					GenderId = userRolesUser.user.GenderId,
					RoleId = role.Id,
					RoleName = role.Name,
				}).Where(x => roles.Contains(x.RoleName)).ToList();

			return users;
		}

		/// <summary>
		/// Retrieves user by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public UserDTO GetUserById(string id)
		{
			var user = _appIdentityDbContext.Users.FirstOrDefault(x => x.Id == id);

			UserDTO userDTO = new();

			if (user != null)
			{
				userDTO.Id = user.Id;
				userDTO.Email = user.Email;
				userDTO.FirstName = user.FirstName;
				userDTO.LastName = user.LastName;
				userDTO.Username = user.UserName;
				userDTO.Address1 = user.Address1;
				userDTO.Address2 = user.Address2;
				userDTO.Address3 = user.Address3;
				userDTO.City = user.City;
				userDTO.Country = user.Country;
				userDTO.County = user.County;
				userDTO.DateOfBirth = user.DateOfBirth;
				userDTO.Email = user.Email;
				userDTO.GenderId = user.GenderId;
				userDTO.Username = user.UserName;
			}

			return userDTO;
		}

		#endregion
	}
}
