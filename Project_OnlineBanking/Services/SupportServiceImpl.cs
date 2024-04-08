using Project_OnlineBanking.Models;
using System;

namespace Project_OnlineBanking.Services
{
    public class SupportServiceImpl : SupportService
    {
        private DatabaseContext db;

        public SupportServiceImpl(DatabaseContext _db)
        {
            db = _db;
        }

        public bool EditStatus(int id, string status)
        {
            var support = db.SupportTickets.Find(id);
            try
            {

                support.Status = (status.Equals("true")) ? "false" : "true";
                db.Entry(support).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                return db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public List<SupportTicket> findAll()
        {
            return db.SupportTickets.ToList();
        }

        public SupportTicket findById(int id)
        {
            return db.SupportTickets.Find(id);
        }

        public List<SupportTicket> searchStatus(string status)
        {
            return db.SupportTickets.Where(s => s.Status.Equals(status)).ToList();
        }
    }
}
