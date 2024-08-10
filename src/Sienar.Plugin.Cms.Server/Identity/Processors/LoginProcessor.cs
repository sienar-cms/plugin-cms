﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sienar.Configuration;
using Sienar.Email;
using Sienar.Errors;
using Sienar.Extensions;
using Sienar.Identity.Requests;
using Sienar.Data;
using Sienar.Identity.Data;
using Sienar.Processors;

namespace Sienar.Identity.Processors;

/// <exclude />
public class LoginProcessor : IProcessor<LoginRequest, bool>
{
	private readonly IUserRepository _repository;
	private readonly IPasswordManager _passwordManager;
	private readonly ISignInManager _signInManager;
	private readonly IAccountEmailManager _emailManager;
	private readonly LoginOptions _loginOptions;
	private readonly SienarOptions _appOptions;

	public LoginProcessor(
		IUserRepository repository,
		IPasswordManager passwordManager,
		ISignInManager signInManager,
		IAccountEmailManager emailManager,
		IOptions<LoginOptions> loginOptions,
		IOptions<SienarOptions> appOptions)
	{
		_repository = repository;
		_passwordManager = passwordManager;
		_signInManager = signInManager;
		_emailManager = emailManager;
		_loginOptions = loginOptions.Value;
		_appOptions = appOptions.Value;
	}

	public async Task<OperationResult<bool>> Process(LoginRequest request)
	{
		var user = await _repository.ReadUserByNameOrEmail(
			request.AccountName,
			Filter.WithIncludes(nameof(SienarUser.Roles)));
		if (user == null)
		{
			return new(
				OperationStatus.NotFound,
				message: CmsErrors.Account.LoginFailedNotFound);
		}

		if (user.IsLockedOut())
		{
			return new(
				OperationStatus.Unauthorized,
				message: CmsErrors.Account.LoginFailedLocked);
		}

		if (!await _passwordManager.VerifyPassword(user, request.Password))
		{
			user.LoginFailedCount++;
			if (user.LoginFailedCount >= _loginOptions.MaxFailedLoginAttempts)
			{
				user.LoginFailedCount = 0;
				user.LockoutEnd = DateTime.Now + _loginOptions.LockoutTimespan;
			}

			await _repository.Update(user);

			return new(
				OperationStatus.Unauthorized,
				message: CmsErrors.Account.LoginFailedInvalid);
		}

		// Still check if email is confirmed no matter what
		// That way, users registered when confirmation was
		// required still need to confirm
		if (!user.EmailConfirmed)
		{
			if (_appOptions.EnableEmail)
			{
				await _emailManager.SendWelcomeEmail(user);
				return new(
					OperationStatus.Unauthorized,
					message: CmsErrors.Account.LoginFailedNotConfirmed);
			}

			return new(
				OperationStatus.Unauthorized,
				message: CmsErrors.Account.LoginFailedNotConfirmedEmailDisabled);
		}

		// User is authenticated and able to log in
		user.LoginFailedCount = 0;
		await _repository.Update(user);

		// Save the token to the token cache
		await _signInManager.SignIn(user, request.RememberMe);

		return new(
			OperationStatus.Success,
			true,
			"Logged in successfully");
	}
}