using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services
{
    public interface UserService
    {
        public bool Login(string username, string password); //Đăng nhập
        public bool Create(Account account);// Tạo tài khoản cho user
        public Account findByUsername(string username);// Tìm tài khoản bằng username
        public List<BankAccount> findByAccId(int accountId);// Tìm danh sách các tài khoản bằng Id Account
        public BankAccount findByBankId(int id);// Tìm số tài khoản bằng Id
        public bool BankNumber(BankAccount bankacc);// Tạo số tài khoản cho tài khoản 
        public string findFullnameByAccnum(string accnum);// Tìm tên tài khoản bằng số tài khoản
        public string checkRegister(string username, string email);// Kiểm tra username và email đã tồn tại chưa
        public bool Ticket(SupportTicket ticket);// Tạo phần hỗ trợ
    }
}
