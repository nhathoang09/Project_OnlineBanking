using Project_OnlineBanking.Models;
using System.Diagnostics;

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
            return db.Accounts.Where(u => u.Username == username).SingleOrDefault();
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

        public string findFullnameByAccnum(string accnum)
        {
            try
            {
                var accId = db.BankAccounts.Where(u => u.AccountNumber == accnum).SingleOrDefault().AccountId;
                return db.Accounts.Where(i => i.AccountId == accId).FirstOrDefault().FullName;
            }catch
            {
                return "Account number does not exist";
            }
        }

        public string checkRegister(string username, string email)
        {
            if (db.Accounts.Where(u => u.Username == username).Count() != 0)
            {
                return "Username";
            }else if (db.Accounts.Where(u => u.Email == email).Count() != 0)
            {
                return "Email";
            }
            else
            {
                return "";
            }
        }

        public bool Ticket(SupportTicket ticket)
        {
            try
            {
                db.SupportTickets.Add(ticket);
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
