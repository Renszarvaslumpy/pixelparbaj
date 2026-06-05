using QRCoder;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace PixelParbaj_CORE.Code
{
    public class RoomManagement
    {
        public enum RoomType
        {
            Duel,
            Party,
            Deathmatch
        }

        private static int IDLength = 12;

        private static Random random = new Random((int)DateTime.Now.Ticks);
        public static string GenerateID()
        {
            const string pool = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            var chars = Enumerable.Range(0, IDLength)
                .Select(x => pool[(random.Next(0, pool.Length))]);
            return new string(chars.ToArray());
        }

        public static string GenerateQRCode(string url, string content)
        {
            string b64 = "";
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                string value = string.Format("{0}api/Pixeletelo/SmartJoin?RoomHash={1}", url, content);
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q))
                using (Base64QRCode qrCode = new Base64QRCode(qrCodeData))
                {
                    b64 = qrCode.GetGraphic(20);
                }
            }
            return b64;
        }

    }
}
