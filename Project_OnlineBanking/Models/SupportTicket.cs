using System;
using System.Collections.Generic;

namespace Project_OnlineBanking.Models;

public partial class SupportTicket
{
    public int TicketId { get; set; }

    public int? AccountId { get; set; }

    public string Subject { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Account { get; set; }
}
