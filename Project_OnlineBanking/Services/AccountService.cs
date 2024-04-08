using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services;

public interface AccountService
{
    public List<Account> findAll();

    public bool create(Account account);

    public bool update(Account account);

    public bool Delete(int id);

    public Account find(string username);

    public Account findById(int id);

    public bool EditStatus(int id, bool status);

    public List<Account> searchStatus(bool status);

    public int countAccounts();



}
