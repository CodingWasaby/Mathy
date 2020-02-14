using System;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Mathy.Client
{
    public class CommonTool
    {
        const string _Key = "MathyWeb";
        const string _IV = "MathyWeb";

        public static void Sendmail(string toAddress, string content, string title)
        {
            string host = "smtp.nim.ac.cn";
            string userName = "crm-service@nim.ac.cn";
            string password = "cmr0916!";

            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = host,
                UseDefaultCredentials = true,
                Credentials = new System.Net.NetworkCredential(userName, password)
            };

            MailMessage msg = new MailMessage
            {
                From = new MailAddress(userName, "U.E.S")
            };
            msg.To.Add(toAddress);
            msg.Subject = title;
            msg.Body = content;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;
            try
            {
                client.Send(msg);
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
        }

        public static string Encrypt(string _strQ)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(_strQ);
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateEncryptor(Encoding.UTF8.GetBytes(_Key), Encoding.UTF8.GetBytes(_IV)), CryptoStreamMode.Write);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray()).Replace("+", "$");
        }

        // 字符串解密
        public static string Decrypt(string _strQ)
        {
            byte[] buffer = Convert.FromBase64String(_strQ.Replace("$", "+"));
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateDecryptor(Encoding.UTF8.GetBytes(_Key), Encoding.UTF8.GetBytes(_IV)), CryptoStreamMode.Write);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}