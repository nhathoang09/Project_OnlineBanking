using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services
{
    public interface TransactionService
    {
        public bool TransferMoney(string sender, string receiver, decimal amount, string message);//Chuyển tiền
        public string mailOTP(string to);// Gửi mail OTP
        public List<Transaction> findByAccountId(int accountId);//  Tra tất cả giao dịch bằng Id người dùng
        public List<Transaction> findByTypeTrans(int accountId, string type);//  Tra các giao dịch chuyển tiền bằng Id người dùng
        public List<Transaction> findByTypeRec(int accountId, string type);//  Tra các giao dịch nhận tiền bằng Id người dùng
        public Decimal AmountUp(int accountId, int month, int year);
        public Decimal AmountDown(int accountId, int month, int year);
        public List<Transaction> findByBankAccountId(int bankAccountId);
        public bool topUp(Transaction transaction);
    }
}
