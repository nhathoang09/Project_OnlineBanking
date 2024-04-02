using Project_OnlineBanking.Models;
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
        public bool Login(string phone, string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.PhoneNumber == phone);
            if (account != null)
            {
                return BCrypt.Net.BCrypt.Verify(password, account.Password);
            }
            return false;
        }
    }
}
