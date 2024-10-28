#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Identity.Requests;
using Sienar.Identity.Results;
using Sienar.Infrastructure;
using Sienar.Services;

namespace Sienar.Identity;

/// <exclude />
[ApiController]
[Route("/api/account")]
[Authorize]
public class AccountController : ServiceController
{
	public AccountController(IOperationResultMapper mapper)
		: base(mapper) {}

	[HttpPost]
	[AllowAnonymous]
	public Task<IActionResult> Register(
		[FromForm] RegisterRequest data,
		[FromServices] IStatusService<RegisterRequest> service)
		=> Execute(() => service.Execute(data));

	[HttpGet]
	public Task<IActionResult> GetAccountData(
		[FromServices] IResultService<AccountDataResult> service)
		=> Execute(service.Execute);

	[HttpPost("confirm")]
	[AllowAnonymous]
	public Task<IActionResult> Confirm(
		[FromForm] ConfirmAccountRequest data,
		[FromServices] IStatusService<ConfirmAccountRequest> service)
		=> Execute(() => service.Execute(data));

	[HttpPost("login")]
	[AllowAnonymous]
	public Task<IActionResult> Login(
		[FromForm] LoginRequest data,
		[FromServices] IStatusService<LoginRequest> service)
		=> Execute(() => service.Execute(data));

	[HttpDelete("login")]
	public Task<IActionResult> Logout(
		[FromForm] LogoutRequest data,
		[FromServices] IStatusService<LogoutRequest> service)
		=> Execute(() => service.Execute(data));

	[HttpDelete("password")]
	[AllowAnonymous]
	public Task<IActionResult> RequestPasswordReset(
		[FromForm] ForgotPasswordRequest data,
		[FromServices] IStatusService<ForgotPasswordRequest> service)
		=> Execute(() => service.Execute(data));

	[HttpPatch("password")]
	[AllowAnonymous]
	public Task<IActionResult> PerformPasswordReset(
		[FromForm] ResetPasswordRequest data,
		[FromServices] IStatusService<ResetPasswordRequest> service)
		=> Execute(() => service.Execute(data));

	[HttpPost("change-email")]
	public Task<IActionResult> ChangeEmail(
		[FromForm] InitiateEmailChangeRequest data,
		[FromServices] IStatusService<InitiateEmailChangeRequest> service)
		=> Execute(() => service.Execute(data));
}
