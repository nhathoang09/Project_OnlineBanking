using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services
{
    public interface UserService
    {
        public bool Login(string username, string password);
        public bool Create(Account account);
        public Account findByUsername(string username);
        public List<BankAccount> findByAccId(int accountId);
        public BankAccount findByBankId(int id);
        public bool BankNumber(BankAccount bankacc);
    }
}
