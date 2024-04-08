using System;
using System.Collections.Generic;

namespace Project_OnlineBanking.Models;

public partial class BankAccount
{
    public int BankAccountId { get; set; }

    public int AccountId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public decimal? Balance { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Cheque> Cheques { get; set; } = new List<Cheque>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
