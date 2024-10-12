using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Identity.Requests;
using Sienar.Infrastructure;
using Sienar.Services;

namespace Sienar.Identity;

/// <exclude />
[ApiController]
[Route("account")]
[Authorize]
public class AccountController : ServiceController
{
	public AccountController(IReadableNotificationService notifier)
		: base(notifier) {}

	[HttpPost]
	[AllowAnonymous]
	public Task<IActionResult> Register(
		[FromForm] RegisterRequest data,
		[FromServices] IStatusService<RegisterRequest> service)
		=> ExecuteService(service, data);
}
