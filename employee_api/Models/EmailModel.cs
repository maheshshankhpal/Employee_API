using System.Net;
using System.Net.Mail;

namespace DynamicReportAPI.Models {
    public class EmailModel {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public  async void sendmail (EmailModel model) {

            using (MailMessage mm = new MailMessage (model.Email, model.To)) {
                mm.Subject = model.Subject;
                mm.Body = model.Body;

                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient ()) {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential (model.Email, model.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;

                    ServicePointManager.ServerCertificateValidationCallback =
                        delegate (object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                            System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
                   await smtp.SendMailAsync(mm);
                }
            }
        }

    }
}