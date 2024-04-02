namespace Project_OnlineBanking.Services
{
    public interface UserService
    {
        public bool Login(string phone, string password);
        public int CheckLock(string phone);
    }
}
