#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Threading.Tasks;
using Sienar.Errors;
using Sienar.Identity.Requests;
using Sienar.Infrastructure;
using Sienar.Data;
using Sienar.Identity.Data;
using Sienar.Processors;

namespace Sienar.Identity.Processors;

/// <exclude />
public class DeleteAccountProcessor : IProcessor<DeleteAccountRequest, bool>
{
	private readonly IUserAccessor _userAccessor;
	private readonly IUserRepository _userRepository;
	private readonly IPasswordManager _passwordManager;

	public DeleteAccountProcessor(
		IUserAccessor userAccessor,
		IUserRepository userRepository,
		IPasswordManager passwordManager)
	{
		_userAccessor = userAccessor;
		_userRepository = userRepository;
		_passwordManager = passwordManager;
	}

	public async Task<OperationResult<bool>> Process(DeleteAccountRequest request)
	{
		var userId = await _userAccessor.GetUserId();
		if (!userId.HasValue)
		{
			return new(
				OperationStatus.Unauthorized,
				message: CmsErrors.Account.LoginRequired);
		}

		var user = await _userRepository.Read(userId.Value);
		if (user is null)
		{
			return new(
				OperationStatus.Unauthorized,
				message: CmsErrors.Account.LoginRequired);
		}

		if (!await _passwordManager.VerifyPassword(user, request.Password))
		{
			return new(
				OperationStatus.Unauthorized,
				message: CmsErrors.Account.LoginFailedInvalid);
		}

		return await _userRepository.Delete(user.Id)
			? new(
				OperationStatus.Success,
				true,
				"Account deleted successfully")
			: new(
				OperationStatus.Unknown,
				false,
				StatusMessages.Database.QueryFailed);
	}
}