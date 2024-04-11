using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services;

public class AccountServiceImpl : AccountService
{
    private DatabaseContext db;

    public AccountServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }

    public bool create(Account account)
    {
        
        try
        {
            db.Accounts.Add(account);
            return db.SaveChanges() > 0;
        } 
        catch
        {
            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            db.Accounts.Remove(db.Accounts.Find(id));
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public List<Account> findAll()
    {
       return db.Accounts.ToList();
    }

    public bool update(Account account)
    {

        if (account != null)
        {
            try
            {
                db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                return db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        return false;
    }

    public Account find(string username)
    {
        return db.Accounts.SingleOrDefault(a=> a.Username == username);
    }

    public Account findById(int id)
    {
        return db.Accounts.Find(id);
    }

    public bool EditStatus(int id, bool status)
    {
        var account = db.Accounts.Find(id);
        try
        {
            account.FailedLoginCount = 0;
            account.IsTransferEnabled = (status)? false : true;
            db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public List<Account> searchStatus(bool status)
    {
        return db.Accounts.Where(a => a.IsTransferEnabled == status).ToList();
    }

    public int countAccounts()
    {
        int count = db.Accounts.Count();
        return count;
    }
}
