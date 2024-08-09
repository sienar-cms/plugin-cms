#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Sienar.Infrastructure;

/// <exclude />
public class DefaultAuthorizationConfigurer : IConfigurer<AuthorizationOptions>
{
	public void Configure(
		AuthorizationOptions options,
		IConfiguration config) {}
}