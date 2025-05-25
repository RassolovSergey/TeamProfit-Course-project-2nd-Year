using Server.DTO.Currency;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Options;

namespace Server.Services.HostedServices
{
    /// <summary>
    /// Фоновая служба для периодического обновления курсов валют из Fixer.io.
    /// </summary>
    public class CurrencyRateUpdaterService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory; // Фабрика для создания HttpClient.
        private readonly IServiceScopeFactory _scopeFactory;    // Фабрика для создания ServiceScope, используемых для получения scoped сервисов (например, DbContext).
        private readonly FixerApiOptions _options;              // Опции для конфигурации доступа к Fixer API.
        private readonly ILogger<CurrencyRateUpdaterService> _logger;   // Логгер для записи событий службы.

        // Конструктор, использующий внедрение зависимостей.
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
        /// Основной метод, выполняющий логику фоновой службы.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Интервал между обновлениями, задается в конфигурации.
            var delay = TimeSpan.FromMinutes(_options.UpdateIntervalMinutes);

            // Цикл работает, пока не получен запрос на остановку службы.
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Логируем запуск обновления курсов
                    _logger.LogInformation("CurrencyRateUpdaterService: Запуск обновления курсов.");

                    // Создаем новый scope для получения DbContext.
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // Формируем и отправляем запрос к API курсов валют.
                    var client = _httpClientFactory.CreateClient("FixerApi");
                    var requestUrl = $"latest?access_key={_options.AccessKey}";
                    _logger.LogInformation("Запрос к API валют: {url}", client.BaseAddress + requestUrl);

                    var response = await client.GetAsync(requestUrl, stoppingToken);
                    response.EnsureSuccessStatusCode(); // Проверяем успешность HTTP ответа.

                    // Десериализуем ответ.
                    var apiResult = await response.Content.ReadFromJsonAsync<FixerApiResponse>(cancellationToken: stoppingToken);

                    // Проверяем, что API вернуло корректные данные.
                    if (apiResult?.Rates == null || !apiResult.Success)
                    {
                        _logger.LogWarning("CurrencyRateUpdaterService: API вернуло некорректные или пустые данные о курсах.");
                    }
                    else
                    {
                        // Загружаем текущие валюты из БД.
                        var currencies = await db.Currencies.ToListAsync(stoppingToken);

                        // Обновляем курсы для каждой валюты.
                        foreach (var cur in currencies)
                        {
                            if (string.Equals(cur.Code, apiResult.Base, StringComparison.OrdinalIgnoreCase))
                            {
                                cur.RateToBase = 1m; // Курс базовой валюты к самой себе равен 1.
                            }
                            else if (apiResult.Rates.TryGetValue(cur.Code, out var rate))
                            {
                                cur.RateToBase = rate; // Обновляем курс из ответа API.
                            }
                        }

                        await db.SaveChangesAsync(stoppingToken); // Сохраняем изменения в БД.
                        _logger.LogInformation("CurrencyRateUpdaterService: Курсы валют успешно обновлены.");
                    }
                }
                // Обрабатываем ошибки, если они не связаны с отменой операции.
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "CurrencyRateUpdaterService: Произошла ошибка при обновлении курсов.");
                }

                // Ожидаем перед следующей итерацией, если служба не остановлена.
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}