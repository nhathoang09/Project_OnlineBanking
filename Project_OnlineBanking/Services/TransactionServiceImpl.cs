using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OtpNet;
using Project_OnlineBanking.Models;
using System.Diagnostics;

namespace Project_OnlineBanking.Services
{
    public class TransactionServiceImpl : TransactionService
    {
        private DatabaseContext db;
        private UserService userService;
        private MailService mailService;
        private bool check = false;

        public TransactionServiceImpl(DatabaseContext _db, UserService _userService, MailService _mailService)
        {
            db = _db;
            userService = _userService;
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
            var remainingTime = totp.RemainingSeconds();

            Debug.WriteLine($"Mã OTP: {result}");
            Debug.WriteLine($"Thời gian còn lại: {remainingTime} giây");

            var from = "lenhath6@gmail.com";
            var subject = "Finbank";
            var content =   "Dear Customer," +
                            "Your One Time Passcode for completing your transaction " +
                            "##transaction## is：## " + int.Parse(result) + " ##" +
                            "Please use this Passcode to complete your transaction. Do not share this passcode with anyone." +
                            "Thank you," +
                            "Finbank" +
                            "Disclaimer: This email and any files transmitted with it are confidential and intended solely for\r\nthe use of the individual or entity to whom they are addressed.";
            if (mailService.Send(from, to, subject, content))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private void LogTransaction(int senderAccountId, int recipientAccountId, decimal amount, string message)
        {
            Transaction transaction = new Transaction
            {
                SenderAccountId = senderAccountId,
                RecipientAccountId = recipientAccountId,
                Amount = amount,
                TransactionType = "Funds Transfer",
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

        
    }
}
