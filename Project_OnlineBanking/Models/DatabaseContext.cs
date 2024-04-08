using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_OnlineBanking.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<Cheque> Cheques { get; set; }

    public virtual DbSet<Helper> Helpers { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SupportTicket> SupportTickets { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA586C2D8EEF8");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastLoginSuccess).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Accounts_Roles");
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.BankAccountId).HasName("PK__BankAcco__4FC8E741F2EA67F4");

            entity.Property(e => e.BankAccountId).HasColumnName("BankAccountID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_BankAccounts_Accounts");
        });

        modelBuilder.Entity<Cheque>(entity =>
        {
            entity.HasKey(e => e.ChequeId).HasName("PK__Cheques__B816D9D044BBBC46");

            entity.Property(e => e.ChequeId).HasColumnName("ChequeID");
            entity.Property(e => e.AccountBankId).HasColumnName("AccountBank_ID");
            entity.Property(e => e.AccountRecipientId).HasColumnName("AccountRecipient_ID");
            entity.Property(e => e.AccountSenderId).HasColumnName("AccountSender_ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.ExpiryDate).HasColumnName("Expiry_date");

            entity.HasOne(d => d.AccountBank).WithMany(p => p.Cheques)
                .HasForeignKey(d => d.AccountBankId)
                .HasConstraintName("FK_Cheques_BankAccounts");
        });

        modelBuilder.Entity<Helper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Helper__3214EC27F14C86F7");

            entity.ToTable("Helper");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Requests__33A8519A0290C149");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.AccountId).HasColumnName("Account_ID");
            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.RequestType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Requests)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Requests_Accounts");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A767A5BEC");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__SupportT__712CC627E42AABC6");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.AccountId).HasColumnName("Account_ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Status)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.SupportTickets)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_SupportTickets_Accounts");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B711F8FB8");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.RecipientAccountId).HasColumnName("RecipientAccount_ID");
            entity.Property(e => e.SenderAccountId).HasColumnName("SenderAccount_ID");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.SenderAccount).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.SenderAccountId)
                .HasConstraintName("FK_Transactions_BankAccounts_Sender");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
