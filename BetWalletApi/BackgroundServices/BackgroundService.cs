namespace BetWalletApi.BackgroundServices
{
    /// <summary>
    /// Base class for background services
    /// </summary>
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCancellationTokens = 
                                                 new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);
 
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCancellationTokens.Token);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if(_executingTask == null)
            {
                return;
            }

            try
            {
                _stoppingCancellationTokens.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public virtual void Dispose()
        {
            _stoppingCancellationTokens.Cancel();
        }
    }
}
