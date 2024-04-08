using System;
using System.Collections.Generic;

namespace Project_OnlineBanking.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int FailedLoginCount { get; set; }

    public DateTime? LastLoginSuccess { get; set; }

    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? IsTransferEnabled { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
}
