using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Configuration
{
	/// <summary>
	/// Configuration class for the identity server that protects the resources and allows clients to access them securely
	/// </summary>
	public static class IdentityResourceConfiguration
	{
		#region Public Methods

		/// <summary>
		/// Gets the list of the identity server resources such as OpenId, Email, and Profile
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Email(),
				new IdentityResources.Profile(),
			};
		}

		/// <summary>
		/// Gets the list of the scopes for which the resources needs to be protected
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ApiScope> GetApiScopes()
		{
			return new List<ApiScope>
			{
				new ApiScope( "otpapi", "OTP API" ),
				new ApiScope( "otpportalapi", "OTP Portal API" ),
				new ApiScope( IdentityServerConstants.LocalApi.ScopeName )
			};
		}

		/// <summary>
		/// Gets the list of the resources for the different Apis
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new ApiResource( "otpapi", "OTP API" )
				{
					Scopes = new []{ "otpapi" }
				},
				new ApiResource( "otpportalapi", "OTP Portal API" )
				{
					Scopes = new []{ "otpportalapi" }
				},
				new ApiResource( IdentityServerConstants.LocalApi.ScopeName )
				{
					Scopes = new []{ IdentityServerConstants.LocalApi.ScopeName }
				}
			};
		}

		/// <summary>
		/// Gets the list of all clients and their corresponding configuration settings
		/// </summary>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static IEnumerable<Client> GetClients(IConfiguration configuration)
		{
			var portalSite = configuration ["PortalSite"];

			return new []
			{
				new Client {
					RequireConsent = false,
					ClientId = "otp_portal",
					ClientName = "OTP Portal",
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowedScopes = { "openid", "profile", "email", "otpportalapi", "role", IdentityServerConstants.LocalApi.ScopeName },
					RedirectUris = { portalSite + "/auth-callback", portalSite + "/silent-refresh.html", portalSite + "/profile" },
					PostLogoutRedirectUris = { portalSite },
					AllowedCorsOrigins = { portalSite },
					AllowAccessTokensViaBrowser = true,
					AccessTokenLifetime = 200,
					IdentityTokenLifetime = 200
				},
			};
		}

		#endregion
	}
}
