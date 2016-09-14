using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DESSAU.ControlGestion.EmisorCorreo
{
    public  class EmisorCorreo
    {
        public static void EnviarCorreo(Correo correo)
        {
            SmtpClient smtpClient = new SmtpClient();
            string Host = "smtp.zoho.com";
            int Port = 587;
            string UserCredential = "soporte@commonabstract.cl";
            string PasswordCredential = "nito110100";

            MailMessage mail = new MailMessage();
            if (correo.Remitente != null)
                mail.From = new MailAddress(correo.Remitente);
            mail.To.Add(new MailAddress(correo.Destinatario));
            if (correo.CC != null)
                mail.CC.Add(new MailAddress(correo.CC));
            if (correo.BCC != null)
                mail.Bcc.Add(new MailAddress(correo.BCC));
            mail.Subject = correo.Asunto;
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(correo.CuerpoTexto, null, "text/plain"));
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(correo.CuerpoHTML, null, "text/html"));

            smtpClient.UseDefaultCredentials = true;
            smtpClient.Host = Host;
            smtpClient.Port = Port;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(UserCredential, PasswordCredential);
            smtpClient.Send(mail);
        }
    }
}
