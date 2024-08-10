#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;

namespace Sienar.Infrastructure;

/// <exclude />
public class DefaultAuthenticationConfigurer : IConfigurer<AuthenticationOptions>
{
	public void Configure(
		AuthenticationOptions options,
		IConfiguration config)
	{
		options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	}
}