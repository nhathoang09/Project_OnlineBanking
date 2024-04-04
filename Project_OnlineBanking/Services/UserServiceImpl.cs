using Project_OnlineBanking.Models;
using System.Diagnostics;
using System.Numerics;

namespace Project_OnlineBanking.Services
{
    public class UserServiceImpl : UserService
    {
        private DatabaseContext db;

        public UserServiceImpl(DatabaseContext _db)
        {
            db = _db;
        }
        public bool Login(string username, string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.Username == username);
            if (account != null)
            {
                return BCrypt.Net.BCrypt.Verify(password, account.Password);
            }
            return false;
        }

        public bool Create(Account account)
        {
            try
            {
                db.Accounts.Add(account);
                return db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public Account findByUsername(string username)
        {
            return db.Accounts.Where(u => u.Username == username).FirstOrDefault();
        }

        public List<BankAccount> findByAccId(int accountId)
        {
            return db.BankAccounts.Where(i => i.AccountId == accountId).ToList();
        }

        public BankAccount findByBankId(int id)
        {
            return db.BankAccounts.Find(id);
        }

        public bool BankNumber(BankAccount bankacc)
        {
            try
            {
                db.BankAccounts.Add(bankacc);
                return db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
