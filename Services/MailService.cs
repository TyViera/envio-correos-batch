using envio_correos_batch.Model;
using System.Net;
using System.Net.Mail;

namespace envio_correos_batch.Services
{
    public class MailService
    {
        private static SmtpClient _smtpServer;

        public static void ConnectToServer(ServerMailModel serverMailModel)
        {
            _smtpServer = new SmtpClient
            {
                Host = serverMailModel.Host,
                Port = 587,
                Credentials = new NetworkCredential()
                {
                    UserName = serverMailModel.User,
                    Password = serverMailModel.Password
                },
                EnableSsl = true
            };

        }

        public static void SendMail(SendMailRow mailRow, DataModel dataModel)
        {
            MailMessage mail = new()
            {
                Subject = mailRow.Message,
                From = new MailAddress(dataModel.ServerMail.User),
                IsBodyHtml = false,
                Body = mailRow.Message
            };
            mail.To.Add(mailRow.Email);

            var pdfRoute = string.Format("{0}{1}{2}", dataModel.FileRoutes.PdfRoute, Path.DirectorySeparatorChar, mailRow.PdfFileName);
            mail.Attachments.Add(new Attachment(pdfRoute));
            if (!string.IsNullOrEmpty(mailRow.XmlFileName))
            {
                var xmlRoute = string.Format("{0}{1}{2}", dataModel.FileRoutes.XmlRoute, Path.DirectorySeparatorChar, mailRow.XmlFileName);
                mail.Attachments.Add(new Attachment(xmlRoute));
            }
            _smtpServer.Send(mail);
        }

    }
}
