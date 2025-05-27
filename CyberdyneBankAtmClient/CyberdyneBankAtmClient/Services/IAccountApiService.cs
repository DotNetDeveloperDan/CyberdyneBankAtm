using CyberdyneBankAtmClient.Models;

namespace CyberdyneBankAtmClient.Services
{
    // Services/IAccountApiService.cs
    public interface IAccountApiService
    {
        Task<AccountDto[]> GetAccountsAsync();
        Task<AccountDto> GetAccountAsync(int accountId);
        Task<TransactionDto[]> GetTransactionsAsync(int accountId);
        Task DepositAsync(CreateTransactionRequest request);
        Task WithdrawAsync(CreateTransactionRequest request);
        Task TransferAsync(CreateTransferRequest request);
    }

}
