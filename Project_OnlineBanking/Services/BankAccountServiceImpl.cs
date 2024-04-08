using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services;

public class BankAccountServiceImpl : BankAccountService
{
    private DatabaseContext db;

    public BankAccountServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }

    public bool addAmount(BankAccount bankAccount)
    {
        try
        {
            db.Entry(bankAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public int countAccountBank()
    {
        int count = db.BankAccounts.Count();
        return count;
    }

    public int countAccountBlock()
    {
        int countblock = db.Accounts.Count(a => a.IsTransferEnabled == false);
        return countblock;
    }

    public List<BankAccount> findByAccountId(int AccountId)
    {
        var accountId = AccountId;
        return db.BankAccounts.Where(b => b.AccountId == AccountId).ToList();
    }

    public BankAccount findByAccountNumber(string accountNumber)
    {
        return db.BankAccounts.SingleOrDefault(b => b.AccountNumber == accountNumber);
    }

    public BankAccount findById(int BankAccountId)
    {
        return db.BankAccounts.Find(BankAccountId);
    }
}
