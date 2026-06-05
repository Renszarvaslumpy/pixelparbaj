using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PixelParbaj_CORE.Code
{
    
    public class UserManagement
    {
        private readonly IConfiguration _configuration;

        public static string PUBLIC = "yOtU7noE";
        public static string SECRET = "d2ABu1yx";

        // Activation Email
        public static string SMTP = "smtp.gmail.com";
        public static string EMAIL = "pixelparbaj@gmail.com";
        public static string PW = "sowuqnocsrbwlfbv";

        private static Random random = new Random();

        private static string[] adjectives = { "Dancing", "Lazy", "Wobbly", "Crazy", "Jumpy", "Smelly", "Goofy", "Sleepy", "Funky", "Noisy" };
        private static string[] nouns = { "Banana", "Panda", "Unicorn", "Hamster", "Ninja", "Donut", "Turtle", "Penguin", "Zombie", "Potato" };

        public static string GenerateFunnyName()
        {
            string adjective = adjectives[random.Next(adjectives.Length)];
            string noun = nouns[random.Next(nouns.Length)];

            return $"{adjective}{noun}{random.Next(1000, 9999)}"; // random számot hozzáad a név végéhez
        }

        public UserManagement(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string Encrypt(string password)
        {
            string ToReturn = "";
            try
            {
                string textToEncrypt = password;
                string publickey = PUBLIC;
                string secretkey = SECRET;
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                
            }
            return ToReturn;
        }

        public static string Decrypt(string password)
        {
            string ToReturn = "";
            try
            {
                string textToDecrypt = password;
                string publickey = PUBLIC;
                string secretkey = SECRET;
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
            }
            catch (Exception ae)
            {
                
            }
            return ToReturn;
        }

        public int SendResetEmail(string email, string key)
        {
            try
            {
                var smtpClient = new SmtpClient(SMTP)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(EMAIL, PW),
                    EnableSsl = true,
                };

                string domainName = _configuration["AppSettings:Domain"];
                string finalURL = domainName + "ResetPassword?Email=" + email + "&Key=" + key;
                string body = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Assets/Verify.html"));
                body = body.Replace("{VERIFY-EMAIL}", finalURL);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(EMAIL),
                    Subject = "PixelPárbaj - Elfelejtett jelszó",
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
                return 0;
            }
            catch (SmtpException smtpex)
            {
                //
            }
            catch (Exception ex)
            {
                //
            }
            return 1;
        }

        public static Models.User GenerateNewAnonymousUser()
        {
            Models.User user = new Models.User();
            user.Name = GenerateFunnyName();
            user.Email = RoomManagement.GenerateID();
            user.Password = RoomManagement.GenerateID();
            user.Avatar = null;
            user.Cheater = 0;
            return user;
        }

    }
}
