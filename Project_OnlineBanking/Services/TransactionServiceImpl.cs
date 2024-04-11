using OtpNet;
using Project_OnlineBanking.Models;
using System.Diagnostics;

namespace Project_OnlineBanking.Services
{
    public class TransactionServiceImpl : TransactionService
    {
        private DatabaseContext db;
        private MailService mailService;

        public TransactionServiceImpl(DatabaseContext _db, UserService _userService, MailService _mailService)
        {
            db = _db;
            mailService = _mailService;
        }

        public bool TransferMoney(string sender, string receiver, decimal amount, string message)
        {
            var BaSender = db.BankAccounts.Where(n => n.AccountNumber == sender).FirstOrDefault();
            var BaReceiver = db.BankAccounts.Where(n => n.AccountNumber == receiver).FirstOrDefault();
            if ((bool)BaSender.Account.IsTransferEnabled)
            {
                // Kiểm tra số dư của người gửi
                if (BaSender.Balance >= amount)
                {
                    // Thực hiện chuyển tiền
                    BaSender.Balance -= amount;
                    BaReceiver.Balance += amount;
                    //Lưu số tiền mới lên db
                    try
                    {
                        db.Entry(BaSender).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.Entry(BaReceiver).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    // Ghi nhật ký giao dịch
                    LogTransaction(BaSender.BankAccountId, BaReceiver.BankAccountId, amount, message);
                    return true;
                }
            }
            return false;
        }

        public string mailOTP(string to)
        {
            var secretKey = "GMZTKNJVGEYTC";
            var bytes = Base32Encoding.ToBytes(secretKey);
            var totp = new Totp(bytes);// Tạo đối tượng Totp
            var result = totp.ComputeTotp();// Tính toán mã OTP

            // Lấy thời gian còn lại cho mã OTP
            /*var remainingTime = totp.RemainingSeconds();
            Debug.WriteLine($"Mã OTP: {result}");
            Debug.WriteLine($"Thời gian còn lại: {remainingTime} giây");*/

            var from = "lenhath6@gmail.com";
            var subject = "Finbank";
            var content = "<h2>Dear Customer,</h2>" +
                            "<h3>Your One Time Passcode for completing your transaction</h3>" +
                            "<h3>Transaction is：##" + result + "##</h3>" +
                            "<h3>Please use this Passcode to complete your transaction. Do not share this passcode with anyone.</h3>" +
                            "<h3>Thank you,</h3>" +
                            "<h2><b>Finbank</b></h2>" +
                            "<h3>Disclaimer: This email and any files transmitted with it are confidential and intended solely for\r\nthe use of the individual or entity to whom they are addressed.</h3>";
            return mailService.Send(from, to, subject, content) ? result : "";
        }

        private void LogTransaction(int senderAccountId, int recipientAccountId, decimal amount, string message)
        {
            Transaction transaction = new Transaction
            {
                SenderAccountId = senderAccountId,
                RecipientAccountId = recipientAccountId,
                Amount = amount,
                TransactionType = "Banking",
                Description = message,
                TransactionDate = DateTime.Now,
            };
            try
            {
                db.Transactions.Add(transaction);
                Debug.WriteLine("Add transaction: " + db.SaveChanges());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            };
        }

        public List<Transaction> findByAccountId(int accountId)
        {
            return db.Transactions.Where(i => i.SenderAccountId == accountId | i.RecipientAccountId == accountId).OrderByDescending(i => i.TransactionDate).ToList();
        }

        public List<Transaction> findByTypeTrans(int accountId, string type)
        {
            return db.Transactions.Where(i => i.SenderAccountId == accountId & i.TransactionType == type).OrderByDescending(i => i.TransactionDate).ToList();
        }

        public List<Transaction> findByTypeRec(int accountId, string type)
        {
            return db.Transactions.Where(i => i.RecipientAccountId == accountId & i.TransactionType == type).OrderByDescending(i => i.TransactionDate).ToList();
        }

        public List<Transaction> findByBankAccountId(int BankAccountId)
        {
            return db.Transactions.Where(t => t.SenderAccountId == BankAccountId || t.RecipientAccountId == BankAccountId).ToList();
        }

        public Decimal AmountUp(int accountId, int month, int year)
        {
            if (month != 0)
            {
                return db.Transactions.Where(i => i.RecipientAccountId == accountId && ((DateTime)i.TransactionDate.Value).Month == month && ((DateTime)i.TransactionDate.Value).Year == year).Sum(a => a.Amount);
            }
            else
            {
                return db.Transactions.Where(i => i.RecipientAccountId == accountId && ((DateTime)i.TransactionDate.Value).Year == year).Sum(a => a.Amount);
            }
        } 

        public Decimal AmountDown(int accountId, int month, int year)
        {
            if (month != 0)
            {
                return db.Transactions.Where(i => i.SenderAccountId == accountId && ((DateTime)i.TransactionDate.Value).Month == month && ((DateTime)i.TransactionDate.Value).Year == year).Sum(a => a.Amount);
            }
            else
            {
                return db.Transactions.Where(i => i.SenderAccountId == accountId && ((DateTime)i.TransactionDate.Value).Year == year).Sum(a => a.Amount);
            }
        }

        public bool topUp(Transaction transaction)
        {
            try
            {
                db.Transactions.Add(transaction);
                return db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
