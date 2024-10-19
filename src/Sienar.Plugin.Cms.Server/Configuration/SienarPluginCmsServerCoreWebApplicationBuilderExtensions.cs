using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sienar.Email;
using Sienar.Extensions;
using Sienar.Hooks;
using Sienar.Identity;
using Sienar.Identity.Hooks;
using Sienar.Identity.Processors;
using Sienar.Identity.Requests;
using Sienar.Identity.Results;
using Sienar.Infrastructure;
using Sienar.Media;
using Sienar.Media.Hooks;
using Sienar.Services;

namespace Sienar.Configuration;

/// <summary>
/// Contains <see cref="WebApplicationBuilder"/> extension methods for the <c>Sienar.Utils</c> assembly
/// </summary>
public static class SienarPluginCmsServerCoreWebApplicationBuilderExtensions
{
	/// <summary>
	/// Adds Sienar server-side services
	/// </summary>
	/// <param name="self">the web application builder</param>
	public static void AddSienarServerCore(
		this WebApplicationBuilder self)
	{
		SienarUtils.SetupBaseDirectory();

		var services = self.Services;
		var config = self.Configuration;

		services
			.AddHttpContextAccessor();

		services.TryAddScoped<IBotDetector, BotDetector>();
		services.TryAddScoped<IEmailSender, DefaultEmailSender>();
		services.TryAddScoped<IPasswordHasher<SienarUser>, PasswordHasher<SienarUser>>();
		services.TryAddScoped<IPasswordManager, PasswordManager>();
		services.TryAddScoped<IUserClaimsFactory, UserClaimsFactory>();
		services.TryAddScoped<IUserClaimsPrincipalFactory<SienarUser>, UserClaimsPrincipalFactory>();
		services.TryAddScoped<IReadableNotificationService, RestNotificationService>();
		services.TryAddScoped<INotificationService>(
			sp => sp.GetRequiredService<IReadableNotificationService>());


		/************
		 * Identity *
		 ***********/

		services.TryAddScoped<IUserAccessor, HttpContextUserAccessor>();
		services.TryAddScoped<IAccountEmailMessageFactory, AccountEmailMessageFactory>();
		services.TryAddScoped<IAccountEmailManager, AccountEmailManager>();
		services.TryAddScoped<IAccountUrlProvider, AccountUrlProvider>();

		// CRUD
		services
			.AddAccessValidator<SienarUser, UserIsAdminAccessValidator<SienarUser>>()
			.AddBeforeHook<SienarUser, UserPasswordUpdateHook>()
			.AddStateValidator<SienarUser, EnsureAccountInfoUniqueValidator>()
			.AddBeforeHook<SienarUser, RemoveUserRelatedEntitiesHook>()

		// Security
			.AddStatusProcessor<LoginRequest, LoginProcessor>()
			.AddStatusProcessor<LogoutRequest, LogoutProcessor>()
			.AddResultProcessor<PersonalDataResult, PersonalDataProcessor>()
			.AddStatusProcessor<AddUserToRoleRequest, UserRoleChangeProcessor>()
			.AddAccessValidator<AddUserToRoleRequest, UserIsAdminAccessValidator<AddUserToRoleRequest>>()
			.AddStatusProcessor<RemoveUserFromRoleRequest, UserRoleChangeProcessor>()
			.AddAccessValidator<RemoveUserFromRoleRequest, UserIsAdminAccessValidator<RemoveUserFromRoleRequest>>()
			.AddStatusProcessor<LockUserAccountRequest, LockUserAccountProcessor>()
			.AddAccessValidator<LockUserAccountRequest, UserIsAdminAccessValidator<LockUserAccountRequest>>()
			.AddStatusProcessor<UnlockUserAccountRequest, UnlockUserAccountProcessor>()
			.AddAccessValidator<UnlockUserAccountRequest, UserIsAdminAccessValidator<UnlockUserAccountRequest>>()
			.AddStatusProcessor<ManuallyConfirmUserAccountRequest, ManuallyConfirmUserAccountProcessor>()
			.AddAccessValidator<ManuallyConfirmUserAccountRequest, UserIsAdminAccessValidator<ManuallyConfirmUserAccountRequest>>()
			.AddStatusProcessor<ChangePasswordRequest, ChangePasswordProcessor>()
			.AddStatusProcessor<ForgotPasswordRequest, ForgotPasswordProcessor>()
			.AddStatusProcessor<ResetPasswordRequest, ResetPasswordProcessor>()
			.AddStatusProcessor<AccessTokenRequest, AccessTokenProcessor>()

		// Registration
			.AddStateValidator<RegisterRequest, RegistrationOpenValidator>()
			.AddStateValidator<RegisterRequest, AcceptTosValidator>()
			.AddStateValidator<RegisterRequest, EnsureAccountInfoUniqueValidator>()
			.AddStatusProcessor<RegisterRequest, RegisterProcessor>()

		// Email
			.AddStatusProcessor<ConfirmAccountRequest, ConfirmAccountProcessor>()
			.AddStatusProcessor<InitiateEmailChangeRequest, InitiateEmailChangeProcessor>()
			.AddStatusProcessor<PerformEmailChangeRequest, PerformEmailChangeProcessor>()

		// Personal data
			.AddBeforeHook<DeleteAccountRequest, RemoveUserRelatedEntitiesHook>()
			.AddStatusProcessor<DeleteAccountRequest, DeleteAccountProcessor>();


		/********
		 * Auth *
		 *******/

		services.TryAddScoped<ISignInManager, CookieSignInManager>();
		services
			.AddConfiguration(new DefaultAuthorizationConfigurer())
			.AddConfiguration(new DefaultAuthenticationConfigurer())
			.AddConfiguration(new DefaultAuthenticationBuilderConfigurer());


		/*********
		 * Media *
		 ********/

		services.TryAddScoped<IMediaDirectoryMapper, MediaDirectoryMapper>();
		services.TryAddScoped<IMediaManager, MediaManager>();

		services
			.AddAccessValidator<Upload, VerifyUserCanReadFileHook>()
			.AddAccessValidator<Upload, VerifyUserCanModifyFileHook>()
			.AddAccessValidator<Upload, VerifyUserCanModifyFileHook>()
			.AddBeforeHook<Upload, AssignMediaFieldsHook>()
			.AddBeforeHook<Upload, UploadFileHook>();


		/***********
		 * Options *
		 **********/

		services
			.ApplyDefaultConfiguration<SienarOptions>(config.GetSection("Sienar:Core"))
			.ApplyDefaultConfiguration<EmailSenderOptions>(config.GetSection("Sienar:Email:Sender"))
			.ApplyDefaultConfiguration<IdentityEmailSubjectOptions>(config.GetSection("Sienar:Email:IdentityEmailSubjects"))
			.ApplyDefaultConfiguration<LoginOptions>(config.GetSection("Sienar:Login"));
	}
}