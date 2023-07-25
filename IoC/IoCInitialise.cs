using IdentityServer.Interfaces.AccountServices;
using IdentityServer.Interfaces.UserServices;
using IdentityServer.Services.AccountServices;
using IdentityServer.Services.UserSerices;
using IdentityServer.Services.UserServices;

using IdentityServer4.Services;

namespace IdentityServer.IoC
{
	public static class IoCInitialise
	{
		public static void Initialise(IServiceCollection services)
		{
			services.AddTransient<IConfirmEmailService, ConfirmEmailService>();
			services.AddTransient<IBuildLoggedOutViewModelService, BuildLoggedOutViewModelService>();
			services.AddTransient<IBuildLoginViewModelService, BuildLoginViewModelService>();
			services.AddTransient<IBuildLogoutViewModelService, BuildLogoutViewModelService>();
			services.AddTransient<ISendConfirmationEmailService, SendConfirmationEmailService>();
			services.AddTransient<ISendCreatePasswordEmailService, SendCreatePasswordEmailService>();
			services.AddTransient<ISendForgottenPasswordEmailService, SendForgottenPasswordEmailService>();

			services.AddTransient<ISetUserAccessRightsService, SetUserAccessRightsService>();
			services.AddTransient<IGetAllUsersService, GetAllUsersService>();
			services.AddTransient<IProfileService, IdentityClaimsProfileService>();
			services.AddTransient<ISetUserActiveStatusService, SetUserActiveStatusService>();
		}
	}
}
