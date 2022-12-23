using Newtonsoft.Json;
using System.Text;

namespace TestMicroservice
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            string email = "krylov.em2002@gmail.com";
            string subject = "ѕодтверждение за€вки на прием ко врачу";
            string message = "¬аша за€вка на прием ко врачу подтверждена регистратурой";
            using (var client = new HttpClient())
            {
                var endPoint = new Uri("https://localhost:7262/Approved/SendEmailNotification");
                var notificationEmail = new NotificationEmail()
                {
                    Email = email,
                    Subject = subject,
                    Message = message,
                };
                var newPostJson = JsonConvert.SerializeObject(notificationEmail);
                var payload = new StringContent(newPostJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endPoint, payload).Result.Content.ReadAsStringAsync().Result;
            }
            Assert.Pass();
        }
    }

    internal class NotificationEmail
    {
        public NotificationEmail()
        {
        }

        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}