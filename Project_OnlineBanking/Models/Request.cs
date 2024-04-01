using System;
using System.Collections.Generic;

namespace Project_OnlineBanking.Models;

public partial class Request
{
    public int RequestId { get; set; }

    public int? AccountId { get; set; }

    public string? RequestType { get; set; }

    public DateTime? RequestDate { get; set; }

    public bool? IsCompleted { get; set; }

    public virtual Account? Account { get; set; }
}
