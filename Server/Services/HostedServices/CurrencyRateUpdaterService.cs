using Server.DTO.Currency; // FixerApiResponse
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Options;

namespace Server.Services.HostedServices
{
    /// <summary>
    /// Фоновая служба, которая по расписанию подтягивает
    /// свежие курсы валют из Fixer.io и обновляет
    /// поле RateToBase в таблице Currencies.
    /// </summary>
    public class CurrencyRateUpdaterService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly FixerApiOptions _options;
        private readonly ILogger<CurrencyRateUpdaterService> _logger;

        public CurrencyRateUpdaterService(
            IHttpClientFactory httpClientFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<FixerApiOptions> options,
            ILogger<CurrencyRateUpdaterService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Главный цикл фоновой службы.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delay = TimeSpan.FromMinutes(_options.UpdateIntervalMinutes);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("CurrencyRateUpdaterService: Начало обновления курсов.");

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var client = _httpClientFactory.CreateClient("FixerApi");
                    var requestUrl = $"latest?access_key={_options.AccessKey}";
                    _logger.LogInformation("Currency API request: {url}", client.BaseAddress + requestUrl);

                    var response = await client.GetAsync(requestUrl, stoppingToken);
                    response.EnsureSuccessStatusCode();

                    // Десериализация FixerApiResponse!
                    var apiResult = await response.Content.ReadFromJsonAsync<FixerApiResponse>(cancellationToken: stoppingToken);

                    if (apiResult?.Rates == null || !apiResult.Success)
                    {
                        _logger.LogWarning("CurrencyRateUpdaterService: получен пустой список курсов или ошибка от API.");
                    }
                    else
                    {
                        var currencies = await db.Currencies.ToListAsync(stoppingToken);

                        foreach (var cur in currencies)
                        {
                            if (string.Equals(cur.Code, apiResult.Base, StringComparison.OrdinalIgnoreCase))
                            {
                                cur.RateToBase = 1m;
                            }
                            else if (apiResult.Rates.TryGetValue(cur.Code, out var rate))
                            {
                                cur.RateToBase = rate;
                            }
                        }

                        await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("CurrencyRateUpdaterService: Курсы успешно обновлены.");
                    }
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "CurrencyRateUpdaterService: Ошибка при обновлении курсов.");
                }

                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
