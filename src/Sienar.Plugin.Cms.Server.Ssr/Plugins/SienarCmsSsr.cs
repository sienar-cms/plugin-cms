using System;
using Microsoft.AspNetCore.Builder;
using Sienar.Configuration;
using Sienar.Extensions;
using Sienar.Infrastructure;

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

	/// <exclude />
	public void SetupApp(WebApplication app)
	{
		app.Services
			.ConfigureScripts(SetupScripts)
			.ConfigureStyles(SetupStyles);
	}

	private static void SetupScripts(IScriptProvider sp)
	{
		sp.Add(new()
		{
			Src = "https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/js/bootstrap.min.js",
			CrossOriginMode = CrossOriginMode.Anonymous,
			ReferrerPolicy = ReferrerPolicy.NoReferrer,
			Integrity = "sha512-WW8/jxkELe2CAiE4LvQfwm1rajOS8PHasCCx+knHG0gBHt8EXxS6T6tJRTGuDQVnluuAvMxWF4j8SNFDKceLFg==",
			ShouldDefer = true
		});
	}

	private void SetupStyles(IStyleProvider sp)
	{
		sp.Add(new()
		{
			Name = "Bootstrap",
			Href = "https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/css/bootstrap.min.css",
			CrossOriginMode = CrossOriginMode.Anonymous,
			ReferrerPolicy = ReferrerPolicy.NoReferrer,
			Integrity = "sha512-b2QcS5SsA8tZodcDtGRELiGv5SaKSk1vDHDaQRda0htPYWZ6046lr3kJ5bAAQdpV2mmA/4v0wQF9MyU6/pDIAg=="
		});
		sp.Add(new()
		{
			Name = "FontAwesome",
			Href = "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css",
			CrossOriginMode = CrossOriginMode.Anonymous,
			ReferrerPolicy = ReferrerPolicy.NoReferrer,
			Integrity = "sha512-z3gLpd7yknf1YoNbCzqRKc4qyor8gaKU1qmn+CShxbuBusANI9QpRohGBreCFkKxLhei6S9CQXFEbbKuqLg0DA=="
		});
		sp.Add(new()
		{
			Name = "Sienar UI",
			Href = $"/_content/Sienar.Ui.Mvc/main.css?v={PluginData.Version}"
		});
	}
}