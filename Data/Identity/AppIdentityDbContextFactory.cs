﻿using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data.Identity
{
	public class AppIdentityDbContextFactory : DesignTimeDbContextFactoryBase<AppIdentityDbContext>
	{
		protected override AppIdentityDbContext CreateNewInstance(DbContextOptions<AppIdentityDbContext> options)
		{
			return new AppIdentityDbContext(options);
		}
	}
}
