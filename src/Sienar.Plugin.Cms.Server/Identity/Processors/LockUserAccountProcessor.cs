﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Threading.Tasks;
using Sienar.Errors;
using Sienar.Identity.Requests;
using Sienar.Data;
using Sienar.Identity.Data;
using Sienar.Processors;

namespace Sienar.Identity.Processors;

/// <exclude />
public class LockUserAccountProcessor : IProcessor<LockUserAccountRequest, bool>
{
	private readonly IUserRepository _userRepository;
	private readonly ILockoutReasonRepository _lockoutReasonRepository;

	public LockUserAccountProcessor(
		IUserRepository userRepository,
		ILockoutReasonRepository lockoutReasonRepository)
	{
		_userRepository = userRepository;
		_lockoutReasonRepository = lockoutReasonRepository;
	}

	public async Task<OperationResult<bool>> Process(LockUserAccountRequest request)
	{
		var user = await _userRepository.Read(
			request.UserId,
			Filter.WithIncludes(nameof(SienarUser.LockoutReasons)));

		if (user is null)
		{
			return new(
				OperationStatus.NotFound,
				message: CmsErrors.Account.NotFound);
		}

		var reasons = await _lockoutReasonRepository.Read(request.Reasons);
		if (reasons.Count != request.Reasons.Count)
		{
			return new(
				OperationStatus.NotFound,
				message: CmsErrors.LockoutReason.NotFound);
		}

		user.LockoutReasons.AddRange(reasons);
		user.LockoutEnd = request.EndDate;

		return await _userRepository.Update(user)
			? new(
				OperationStatus.Success,
				true,
				$"Locked user {user.Username} successfully")
			: new(
				OperationStatus.Unknown,
				message: StatusMessages.Database.QueryFailed);
	}
}