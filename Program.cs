using IdentityServer.Data.Identity;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
	public class Program
	{
		public static void Main(string [] args)
		{
			var host = CreateHostBuilder(args).Build();

			using (IServiceScope serviceScope = host.Services.CreateScope())
			{
				var identityDbcontext = serviceScope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

				identityDbcontext.Database.Migrate();

				var persistentGrantDbcontext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

				persistentGrantDbcontext.Database.Migrate();
			}

			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string [] args) =>
			Host.CreateDefaultBuilder(args)
				.UseContentRoot(Directory.GetCurrentDirectory())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}