using EmailNotifier.PrivateSettings;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotifier.Controllers
{
    [Route("Approved")]
    [ApiController]
    public class AppointmentApproved : ControllerBase
    {
        [HttpPost]
        [Route("SendEmailNotification")]
        public string Send([FromQuery] string email, [FromQuery] string subject, [FromQuery] string message)
        {
            try
            {
                /*
                var initialEmail = new MimeMessage();
                initialEmail.From.Add(MailboxAddress.Parse(MailSettings.Email));
                initialEmail.To.Add(MailboxAddress.Parse(email));
                initialEmail.Subject = "Подтверждение заявки на прием ко врачу";
                initialEmail.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = message };
                */

                var smtpClient = new System.Net.Mail.SmtpClient("smtp.mail.ru", 587);
                smtpClient.Credentials = new System.Net.NetworkCredential(MailSettings.Email, MailSettings.Password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(new System.Net.Mail.MailMessage(MailSettings.Email, email, subject, message));
                smtpClient.Dispose();

                return "Оповещение отправлено";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
