using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services;

public interface BankAccountService
{
    public int countAccountBank();

    public int countAccountBlock();

    public List<BankAccount> findByAccountId(int AccountId);


    public BankAccount findById(int BankAccountId);

    public BankAccount findByAccountNumber(string accountNumber);


    public bool addAmount(BankAccount bankAccount);
    
}
