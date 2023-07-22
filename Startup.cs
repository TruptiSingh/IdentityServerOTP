using IdentityServer.Configuration;
using IdentityServer.Constants;
using IdentityServer.Data.Identity;
using IdentityServer.IoC;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		private readonly IWebHostEnvironment _currentEnvironment;

		public Startup(IConfiguration configuration,
			IWebHostEnvironment env)
		{
			Configuration = configuration;
			_currentEnvironment = env;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			var connectionString = Configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<AppIdentityDbContext>(options =>
				 options.UseSqlServer(connectionString));

			services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<AppIdentityDbContext>()
				.AddDefaultTokenProviders();

			services.AddLocalApiAuthentication();

			var identityServiceBuilder = services.AddIdentityServer(options =>
			{
				options.Authentication.CookieLifetime = TimeSpan.FromMinutes(60);
				options.Authentication.CookieSlidingExpiration = true;
			})
				// this adds the operational data from DB (codes, tokens, consents)
				.AddOperationalStore(options =>
				{
					options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
					// this enables automatic token cleanup. this is optional.
					options.EnableTokenCleanup = true;
					options.TokenCleanupInterval = 3600; // interval in seconds
				})

				//.AddInMemoryPersistedGrants()
				.AddInMemoryIdentityResources(IdentityResourceConfiguration.GetIdentityResources())
				.AddInMemoryApiScopes(IdentityResourceConfiguration.GetApiScopes())
				.AddInMemoryApiResources(IdentityResourceConfiguration.GetApiResources())
				.AddInMemoryClients(IdentityResourceConfiguration.GetClients(Configuration))
				.AddAspNetIdentity<AppUser>();

			services.ConfigureApplicationCookie(options =>
			{
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				options.SlidingExpiration = true;
			});

			identityServiceBuilder.AddDeveloperSigningCredential();

			string [] roles = { Roles.Administrator, Roles.Tutor, Roles.Student };

			services.AddAuthorization(options =>
			{
				options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
				{
					policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
					policy.RequireAuthenticatedUser();
					policy.RequireRole(roles);
				});
			});

			IoCInitialise.Initialise(services);

			services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
				 .AllowAnyMethod()
				 .AllowAnyHeader()));

			services.AddControllers();
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
			UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			UserInitialiser.SeedRoles(roleManager);
			UserInitialiser.SeedUsers(userManager);

			app.UseCors("AllowAll");
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseIdentityServer();

			app.Use(async (ctx, next) =>
			{
				ctx.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; font-src 'self' https://fonts.googleapis.com https://fonts.gstatic.com; style-src 'self' https://fonts.googleapis.com;");

				await next();
			});

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
