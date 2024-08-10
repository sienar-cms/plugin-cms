using Microsoft.Extensions.Options;
using Sienar.Configuration;

namespace Sienar.Identity;

public class AccountUrlProvider : IAccountUrlProvider
{
	private readonly SienarOptions _sienarOptions;

	public AccountUrlProvider(IOptions<SienarOptions> sienarOptions)
	{
		_sienarOptions = sienarOptions.Value;
	}

	/// <inheritdoc />
	public string ConfirmationUrl
		=> $"{_sienarOptions.SiteUrl}/Dashboard/Account/Confirm";

	/// <inheritdoc />
	public string EmailChangeUrl
		=> $"{_sienarOptions.SiteUrl}/Dashboard/Account/Email/Confirm";

	/// <inheritdoc />
	public string ResetPasswordUrl
		=> $"{_sienarOptions.SiteUrl}/Dashboard/Account/ResetPassword";
}