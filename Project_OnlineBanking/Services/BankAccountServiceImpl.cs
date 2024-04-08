using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services;

public class BankAccountServiceImpl : BankAccountService
{
    private DatabaseContext db;

    public BankAccountServiceImpl(DatabaseContext _db)
    {
        db = _db;
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
        return db.BankAccounts.Where(b => b.AccountId == AccountId).ToList();
    }

    public BankAccount findById(int BankAccountId)
    {
        return db.BankAccounts.Find(BankAccountId);
    }
}
