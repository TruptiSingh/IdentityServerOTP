﻿using IdentityServer.DTOs.Roles;
using IdentityServer.DTOs.Users;
using IdentityServer.Interfaces.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.ApiControllers
{
	[ApiController]
	[Authorize(LocalApi.PolicyName)]
	[Route("api/v1/[controller]")]
	public class UsersController : Controller
	{
		private readonly IGetAllUsersService _getAllUsersService;
		private readonly ISetUserAccessRightsService _setUserAdminRightsService;

		public UsersController(IGetAllUsersService getAllUsersService, ISetUserAccessRightsService setUserAccessRightsService)
		{
			_getAllUsersService = getAllUsersService;
			_setUserAdminRightsService = setUserAccessRightsService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
		{
			try
			{
				var users = await _getAllUsersService.GetAllUsers();

				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public ActionResult<UserDTO> GetUserById(string id)
		{
			try
			{
				var user = _getAllUsersService.GetUserById(id);

				return Ok(user);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("UsersByRoles")]
		public ActionResult<IEnumerable<UserDTO>> GetUsersByRoles(RolesDTO rolesDTO)
		{
			try
			{
				var users = _getAllUsersService.GetUsersByRoles(rolesDTO.Roles);

				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut]
		public ActionResult SetUserRole(SetUserAccessDTO setUserAccessDTO)
		{
			try
			{
				_setUserAdminRightsService.SetUserAccessRights(setUserAccessDTO.UserId,
					setUserAccessDTO.RoleName, setUserAccessDTO.SetAccess);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
