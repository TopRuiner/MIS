using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EmailNotifier.Controllers
{
    [Route("Approved")]
    [ApiController]
    public class AppointmentApproved : ControllerBase
    {
        [HttpPost]
        [Route("SendEmailNotification")]
        public string Send([FromQuery] string email, [FromQuery] string message)
        {
            try
            {
                var initialEmail = new MimeMessage();
                initialEmail.From.Add(MailboxAddress.Parse("krylov.em2002@gmail.com"));
                initialEmail.To.Add(MailboxAddress.Parse(email));
                initialEmail.Subject = "Подтверждение заявки на прием ко врачу";
                initialEmail.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = message };
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("krylov.em2002@gmail.com", "rfxbhiedmrmdvvdr");
                    smtp.Send(initialEmail);
                    smtp.Disconnect(true);
                }

                return "Оповещение отправлено";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
