namespace Project_OnlineBanking.Services
{
    public interface MailService
    {
        public bool Send(string from, string to, string subject, string content);
    }
}
