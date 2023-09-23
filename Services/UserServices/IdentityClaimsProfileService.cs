using IdentityModel;
using IdentityServer.Constants;
using IdentityServer.Data.Identity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Services.UserServices
{
	/// <summary>
	/// Class responsible for managing user claims
	/// </summary>
	public class IdentityClaimsProfileService : IProfileService
	{
		#region Private Variables

		private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
		private readonly UserManager<AppUser> _userManager;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Public Constructor
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="claimsFactory"></param>
		public IdentityClaimsProfileService(UserManager<AppUser> userManager,
			IUserClaimsPrincipalFactory<AppUser> claimsFactory)
		{
			_userManager = userManager;
			_claimsFactory = claimsFactory;
		}

		#endregion

		#region Public Mthods

		/// <summary>
		/// Gets the user profile and adds appropriate roles to it depending on the user's current role
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var sub = context.Subject.GetSubjectId();
			var user = await _userManager.FindByIdAsync(sub);
			var principal = await _claimsFactory.CreateAsync(user);

			var claims = principal.Claims.ToList();

			claims.Add(new Claim(JwtClaimTypes.GivenName, string.Concat(user.FirstName, ' ', user.LastName)));

			if (principal.Claims.Any(x => x.Type == JwtClaimTypes.Role &&
				x.Value == Roles.Tutor))
			{
				claims.Add(new Claim(JwtClaimTypes.Role, Roles.Tutor));
			}

			if (principal.Claims.Any(x => x.Type == JwtClaimTypes.Role &&
				x.Value == Roles.Student))
			{
				claims.Add(new Claim(JwtClaimTypes.Role, Roles.Student));
			}

			if (principal.Claims.Any(x => x.Type == JwtClaimTypes.Role &&
				x.Value == Roles.Administrator))
			{
				claims.Add(new Claim(JwtClaimTypes.Role, Roles.Tutor));
				claims.Add(new Claim(JwtClaimTypes.Role, Roles.Student));
			}

			if (!claims.Any(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"))
			{
				claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Id));
			}

			context.IssuedClaims = claims;
		}

		/// <summary>
		/// Checks whether user is active or not
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task IsActiveAsync(IsActiveContext context)
		{
			var sub = context.Subject.GetSubjectId();

			var user = await _userManager.FindByIdAsync(sub);

			context.IsActive = user != null;
		}

		#endregion
	}
}
