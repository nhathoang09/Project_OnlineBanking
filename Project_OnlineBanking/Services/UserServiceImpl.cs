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

        public int CheckLock(string phone)
        {
            return db.Accounts.Where(p => p.PhoneNumber == phone).Select(p => p.FailedLoginCount).FirstOrDefault();
            
        }

        public bool Login(string phone, string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.PhoneNumber == phone);
            if (account != null)
            {
                if (CheckLock(phone) <= 3)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, account.Password)){
                        return true;
                    }
                    else
                    {
                    }
                }
            }
            return false;
        }
    }
}
