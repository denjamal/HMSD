using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Gateway.Settings;

namespace WebApi.Gateway.Background_Tasks
{
    public class RotateKeysBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger<RotateKeysBackgroundService> _logger;
        private readonly EncryptionServiceApi _encryptionServiceApi;
        private Timer _timer;

        public RotateKeysBackgroundService(
            ILogger<RotateKeysBackgroundService> logger,
            EncryptionServiceApi encryptionServiceApi)
        {
            _logger = logger;
            _encryptionServiceApi = encryptionServiceApi;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Rotate Keys Background Service running.");

            _timer = new Timer(RotateKeys, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(30));

            return Task.CompletedTask;
        }

        private void RotateKeys(object state)
        {
            _logger.LogInformation("Rotating keys");

            using var client = GetHttpClient();
            var response = client.PostAsync(_encryptionServiceApi.RotateUrl, null).Result;

            try
            {
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Rotating keys success!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Rotating keys failed!", ex);
            }

        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_encryptionServiceApi.BaseUrl)
            };

            return client;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Rotate Keys Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
