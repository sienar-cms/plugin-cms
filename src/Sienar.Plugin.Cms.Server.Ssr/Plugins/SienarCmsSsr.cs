using System;
using Microsoft.AspNetCore.Builder;
using Sienar.Configuration;

namespace Sienar.Plugins;

/// <summary>
/// Adds Sienar CMS functionality as a traditional server-side rendered web application using ASP.NET MVC
/// </summary>
public class SienarCmsSsr : IWebPlugin
{
	/// <exclude />
	public PluginData PluginData { get; } = new()
	{
		Name = "Sienar CMS (SSR mode)",
		Version = Version.Parse("0.1.0"),
		Author = "Christian LeVesque",
		AuthorUrl = "https://levesque.dev",
		Description = "Sienar CMS (SSR mode) provides all of the main services and configuration required to operate the Sienar CMS as a traditional server-side rendered web application. Sienar cannot function without this plugin.",
		Homepage = "https://sienar.io"
	};

	/// <exclude />
	public void SetupDependencies(WebApplicationBuilder builder)
	{
		builder.AddSienarServerCore();
	}
}