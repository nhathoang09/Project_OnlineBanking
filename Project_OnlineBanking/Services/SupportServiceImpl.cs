using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services
{
    public class SupportServiceImpl : SupportService
    {
        private DatabaseContext db;

        public SupportServiceImpl(DatabaseContext _db)
        {
            db = _db;
        }
    }
}
