namespace PixelParbaj_CORE.Code
{
    public class Strings
    {
        public static List<string> LoginLabels = new List<string>()
    {
        "Lassan a testtel kispajtás, <br />előbb jelentkezz be új párbaj létrehozásához!",
        "Ne olyan gyorsan sporttárs, <br />előbb jelentkezz be új párbaj létrehozásához!",
        "Álljunk meg egy ásónyomra, <br />előbb jelentkezz be új párbaj létrehozásához!",
        "Ne olyan gyorsan csődör, <br />előbb jelentkezz be új párbaj létrehozásához!",
        "Vegyél vissza a lendületből, <br />előbb jelentkezz be új párbaj létrehozásához!"
    };

        public static string GetLoginLabel()
        {
            Random random = new Random();
            string label = LoginLabels[random.Next(0, LoginLabels.Count)];
            return label;
        }
    }
}
