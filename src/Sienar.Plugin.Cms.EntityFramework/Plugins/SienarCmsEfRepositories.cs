using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sienar.Extensions;
using Sienar.Identity;
using Sienar.Identity.Data;
using Sienar.Identity.Hooks;
using Sienar.Identity.Processors;
using Sienar.Media;
using Sienar.Media.Processors;

namespace Sienar.Plugins;

public class SienarCmsEfRepositories<TContext> : IWebPlugin
	where TContext : DbContext
{
	/// <inheritdoc />
	public PluginData PluginData { get; } = new()
	{
		Name = "Sienar CMS Entity Framework Repositories",
		Version = Version.Parse("0.1.0"),
		Author = "Christian LeVesque",
		AuthorUrl = "https://levesque.dev",
		Description = "This plugin provides repositories to persist app data to any database provider supported by Entity Framework",
		Homepage = "https://sienar.levesque.dev"
	};

	/// <inheritdoc />
	public void SetupDependencies(WebApplicationBuilder builder)
	{
		var services = builder.Services;

		services.TryAddScoped<IVerificationCodeManager, VerificationCodeManager<TContext>>();
		services.TryAddScoped<IUserRepository, UserRepository<TContext>>();

		services
			.AddEntityFrameworkEntity<SienarUser, SienarUserFilterProcessor, IUserRepository, UserRepository<TContext>>()
			.AddBeforeHook<SienarUser, FetchNotUpdatedUserPropertiesHook<TContext>>()
			.AddEntityFrameworkEntityWithDefaultRepository<SienarRole, SienarRoleFilterProcessor, TContext>()
			.AddEntityFrameworkEntity<LockoutReason, LockoutReasonFilterProcessor, ILockoutReasonRepository, LockoutReasonRepository<TContext>>()
			.AddEntityFrameworkEntityWithDefaultRepository<Upload, UploadFilterProcessor, TContext>();
	}
}