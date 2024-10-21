﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Identity.Requests;
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
}
