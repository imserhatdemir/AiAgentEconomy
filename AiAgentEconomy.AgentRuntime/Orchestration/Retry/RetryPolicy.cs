using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.Orchestration.Retry
{
    public static class RetryPolicy
    {
        public static async Task ExecuteAsync(
            Func<CancellationToken, Task> action,
            int maxAttempts,
            TimeSpan initialDelay,
            ILogger logger,
            string operationName,
            CancellationToken ct)
        {
            Exception? last = null;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await action(ct);
                    return;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    last = ex;
                    logger.LogWarning(ex,
                        "{Operation} failed. Attempt {Attempt}/{MaxAttempts}",
                        operationName, attempt, maxAttempts);

                    if (attempt == maxAttempts)
                        break;

                    var delay = TimeSpan.FromMilliseconds(initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                    await Task.Delay(delay, ct);
                }
            }

            throw last ?? new Exception($"{operationName} failed.");
        }
    }
}
