using CyberdyneBankAtmClient.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using CyberdyneBankAtmClient.Enums;

namespace CyberdyneBankAtmClient.Services
{
    public class AccountApiService : IAccountApiService
    {
        private readonly HttpClient _http;
        private readonly ILogger<AccountApiService> _logger;

        public AccountApiService(HttpClient http, ILogger<AccountApiService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<AccountDto[]> GetAccountsAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<AccountDto[]>("/accounts?userId=1")
                    ?? Array.Empty<AccountDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error getting accounts.");
                throw new ApplicationException("A network error occurred while retrieving accounts.", ex);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Unsupported content type error getting accounts.");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON error getting accounts.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting accounts.");
                throw;
            }
        }


        public async Task<AccountDto> GetAccountAsync(int accountId)
        {
            try
            {
                return await _http.GetFromJsonAsync<AccountDto>($"/accounts/{accountId}")
                       ??new AccountDto();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error getting account.");
                throw new ApplicationException("A network error occurred while retrieving account.", ex);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Unsupported content type error getting account.");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON error getting account.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting account.");
                throw;
            }

        }

        public async Task<TransactionDto[]> GetTransactionsAsync(int accountId)
        {
            try
            {
                return await _http.GetFromJsonAsync<TransactionDto[]>($"transactions?accountId={accountId}")
                    ?? Array.Empty<TransactionDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error getting transactions for AccountId={AccountId}", accountId);
                throw new ApplicationException("A network error occurred while retrieving transactions.", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON error getting transactions for AccountId={AccountId}", accountId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting transactions for AccountId={AccountId}", accountId);
                throw;
            }
        }

        public async Task DepositAsync(CreateTransactionRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("transactions", request);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during deposit.");
                throw new ApplicationException("A network error occurred during deposit.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during deposit.");
                throw;
            }
        }

        public async Task WithdrawAsync(CreateTransactionRequest request)
        {
            try
            {

                var response = await _http.PostAsJsonAsync("transactions", request);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during withdrawal.");
                throw new ApplicationException("A network error occurred during withdrawal.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during withdrawal.");
                throw;
            }
        }

        public async Task TransferAsync(CreateTransferRequest request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("transactions/transfers", request);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during transfer.");
                throw new ApplicationException("A network error occurred during transfer.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during transfer.");
                throw;
            }
        }

       
    }
}
