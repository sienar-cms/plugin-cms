using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Data;
using Sienar.Hooks;
using Sienar.Infrastructure;
using Sienar.Processors;

namespace Sienar.Services;

public class SienarService<TRequest, TResult> : Service<TRequest, TResult>
{
	private readonly IBotDetector _botDetector;

	/// <inheritdoc />
	public SienarService(
		ILogger<Service<TRequest, TResult>> logger,
		IEnumerable<IAccessValidator<TRequest>> accessValidators,
		IEnumerable<IStateValidator<TRequest>> stateValidators,
		IEnumerable<IBeforeProcess<TRequest>> beforeHooks,
		IEnumerable<IAfterProcess<TRequest>> afterHooks,
		IProcessor<TRequest, TResult> processor,
		INotificationService notifier,
		IBotDetector botDetector)
		: base(
			logger,
			accessValidators,
			stateValidators,
			beforeHooks,
			afterHooks,
			processor,
			notifier)
	{
		_botDetector = botDetector;
	}

	/// <inheritdoc />
	public override Task<OperationResult<TResult?>> Execute(TRequest request)
	{
		if (request is Honeypot honeypot && _botDetector.IsSpambot(honeypot))
		{
			return Task.FromResult(new OperationResult<TResult?>());
		}

		return base.Execute(request);
	}
}