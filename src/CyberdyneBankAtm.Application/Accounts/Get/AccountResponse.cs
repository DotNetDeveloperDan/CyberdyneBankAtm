﻿using CyberdyneBankAtm.Domain.Accounts;

namespace CyberdyneBankAtm.Application.Accounts.Get;

public class AccountResponse
{
    public int AccountId { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedOn { get; set; }
}