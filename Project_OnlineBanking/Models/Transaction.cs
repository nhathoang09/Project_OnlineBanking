using System;
using System.Collections.Generic;

namespace Project_OnlineBanking.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? SenderAccountId { get; set; }

    public int? RecipientAccountId { get; set; }

    public decimal Amount { get; set; }

    public string? TransactionType { get; set; }

    public string? Description { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual BankAccount? SenderAccount { get; set; }
}
