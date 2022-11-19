using System.Net;
using System.Net.Mail;

namespace CalendarBackend
{
    public class SSendEmail : ISendEmail
    {
        public bool sendEmailUsers(string email)
        {
            try
            {
                string fromMail = "ing.ricardo.alex@gmail.com";
                string fromPassword = "ucycjbgrkimevbca";

                MailMessage message = new()
                {
                    From = new MailAddress(fromMail),
                    Subject = "Calendar App"
                };

                message.To.Add(new MailAddress(email));
                message.Body = $"<html>" +
                    $"<body> " +
                    $"el usuario {email} te ha invitado a unirte a un evento, Ingresa a Calendar App para mas detalles " +
                    $"</body> <br/> <a href=\"https://calendarapp-ricardo.netlify.app/\">Calendar App</a> <br/> " +
                    $"</html>";

                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                smtpClient.Send(message);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
