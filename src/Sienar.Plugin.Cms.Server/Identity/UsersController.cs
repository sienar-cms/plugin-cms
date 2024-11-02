#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sienar.Data;
using Sienar.Identity.Requests;
using Sienar.Infrastructure;
using Sienar.Services;

namespace Sienar.Identity;

/// <exclude />
[ApiController]
[Route("/api/users")]
[Authorize(Roles = Roles.Admin)]
public class UsersController : ServiceController
{
	public UsersController(IOperationResultMapper mapper)
		: base(mapper) {}

	[HttpGet]
	public Task<IActionResult> Read(
		[FromQuery] Filter? filter,
		[FromServices] IEntityReader<SienarUser> service)
		=> Execute(() => service.Read(filter));

	[HttpGet("{id:guid}")]
	public Task<IActionResult> Read(
		Guid id,
		[FromQuery] Filter? filter,
		[FromServices] IEntityReader<SienarUser> service)
		=> Execute(() => service.Read(id, filter));

	[HttpPost]
	public Task<IActionResult> Create(
		[FromForm] SienarUser entity,
		[FromServices] IEntityWriter<SienarUser> service)
		=> Execute(() => service.Create(entity));

	[HttpPut]
	public Task<IActionResult> Update(
		[FromForm] SienarUser entity,
		[FromServices] IEntityWriter<SienarUser> service)
		=> Execute(() => service.Update(entity));

	[HttpDelete("{id:guid}")]
	public Task<IActionResult> Delete(
		Guid id,
		[FromServices] IEntityDeleter<SienarUser> service)
		=> Execute(() => service.Delete(id));

	[HttpPost("roles")]
	public Task<IActionResult> AddToRole(
		[FromForm] AddUserToRoleRequest data,
		[FromServices] IStatusService<AddUserToRoleRequest> service)
		=> Execute(() => service.Execute(data));

	[HttpDelete("roles")]
	public Task<IActionResult> RemoveFromRole(
		[FromForm] RemoveUserFromRoleRequest data,
		[FromServices] IStatusService<RemoveUserFromRoleRequest> service)
		=> Execute(() => service.Execute(data));
}