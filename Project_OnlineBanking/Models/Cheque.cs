using System;
using System.Collections.Generic;

namespace Project_OnlineBanking.Models;

public partial class Cheque
{
    public int ChequeId { get; set; }

    public int? AccountSenderId { get; set; }

    public int? AccountRecipientId { get; set; }

    public int? AccountBankId { get; set; }

    public decimal? Amount { get; set; }

    public string? Description { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public bool? IsCancelled { get; set; }

    public virtual BankAccount? AccountBank { get; set; }
}
