using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services
{
    public interface SupportService
    {
        public List<SupportTicket> findAll();
        public SupportTicket findById(int id);

        public bool EditStatus(int id, string status);

        public List<SupportTicket> searchStatus(string status);


    }
}
