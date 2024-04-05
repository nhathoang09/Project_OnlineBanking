namespace Project_OnlineBanking.Services
{
    public interface TransactionService
    {
        public bool TransferMoney(string sender, string receiver, decimal amount, string message);//Chuyển tiền
        public string mailOTP(string to);
    }
}
