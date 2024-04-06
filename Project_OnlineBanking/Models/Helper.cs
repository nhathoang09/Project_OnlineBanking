using System;
using System.Collections.Generic;

namespace Project_OnlineBanking.Models;

public partial class Helper
{
    public int Id { get; set; }

    public string ErrorCode { get; set; } = null!;

    public string Content { get; set; } = null!;
}
