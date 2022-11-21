using System.Net;
using System.Net.Mail;

namespace CalendarBackend
{
    public class SSendEmail : ISendEmail
    {
        private readonly ICOSetting mSetting;
        public SSendEmail(ICOSetting mSetting)
        {
            this.mSetting = mSetting;
        }

        public bool sendEmailUsers(string email)
        {
            try
            {
                var setting = mSetting.AppSettings();

                MailMessage message = new()
                {
                    From = new MailAddress(setting.Email),
                    Subject = "Calendar App"
                };

                message.To.Add(new MailAddress(email));
                message.Body = $"<html>" +
                    $"<body> " +
                    $"el usuario {email} te ha invitado a unirte a un evento, Ingresa a Calendar App para mas detalles " +
                    $"</body> <br/> <a href=\"https://calendarapp-ricardo.netlify.app/\">Calendar App</a> <br/> " +
                    $"</html>";

                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient(setting.Client)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(setting.Email, setting.Pass),
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
