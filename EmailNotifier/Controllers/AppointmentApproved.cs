using EmailNotifier.PrivateSettings;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotifier.Controllers
{
    [Route("Approved")]
    [ApiController]
    public class AppointmentApproved : ControllerBase
    {
        [HttpGet]
        [Route("SendEmailNotification")]
        public string Send([FromQuery] string email, [FromQuery] string subject, [FromQuery] string message)
        {
            try
            {
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
