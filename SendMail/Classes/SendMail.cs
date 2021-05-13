using System;
using System.Configuration;
using System.Net.Mail;

namespace SendMail
{
    public static class SendMail
    {
        public static void SendMailMessagePrices(string _subject, string _body)
        {
            using (MailMessage _msg =
                new MailMessage(ConfigurationManager.AppSettings.Get("mailFrom").ToString(),
                                ConfigurationManager.AppSettings.Get("mailsTo").ToString(),
                                _subject,
                                _body))
            {
                using (SmtpClient smtp = new SmtpClient(
                    ConfigurationManager.AppSettings["smtpHost"],
                    int.Parse(ConfigurationManager.AppSettings["smtpPort"])))
                {
                    try
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials =
                            new System.Net.NetworkCredential(ConfigurationManager.AppSettings["mailFrom"], ConfigurationManager.AppSettings["smtpUsrPwd"]);
                        smtp.EnableSsl = true;
                        smtp.Send(_msg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during try to send email. Message: {ex.ToString()}");
                    }
                }
            }
            
        }
    }
}
