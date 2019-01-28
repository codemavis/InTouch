using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InTouch.Classes.Connection
{
    class Send_Email
    {
        public bool sendEmail(string FileName, string cTo, string cCC, string cBCC, string cSubject, string cBody, string userEMail, string userPass)
        {

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress(userEMail);

                if (!cTo.Trim().Equals(""))
                    message.To.Add(new MailAddress(cTo));
                if (!cCC.Trim().Equals(""))
                    message.CC.Add(new MailAddress(cCC));
                if (!cBCC.Trim().Equals(""))
                    message.Bcc.Add(new MailAddress(cBCC));
                message.Subject = cSubject;
                message.Body = cBody;
                string currentPath = System.IO.Directory.GetCurrentDirectory().Trim();
                message.Attachments.Add(new Attachment(currentPath + "\\" + FileName));

                smtp.Port = 587;
                //smtp.Port = 465;
                //smtp.Port = 2525;
                //smtp.Port = 465;
                //smtp.Host = "smtp.gmail.com";
                smtp.Host = "mail.vsjts.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(userEMail, userPass);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                MessageBox.Show("Email Sent!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("err: " + ex.Message);
            }
            return false;
        }
    }
}
