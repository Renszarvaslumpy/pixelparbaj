using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PixelParbaj_CORE.Code;
using PixelParbaj_CORE.Models;

namespace PixelParbaj_CORE.Pages
{
    public class PixelModel : PageModel
    {
        private Data.IPP ipp { get; set; }
        public PP2Context db { get; set; }
        public string RoomHash { get; set; }
        public Models.User User { get; set; }
        public Models.Room Room { get; set; }
        public List<Join> joins { get; set; }
        public string QrCode { get; set; }

        public PixelModel(PP2Context db, Data.IPP ipp)
        {
            this.ipp = ipp;
            this.db = db;
        }
        public async Task<IActionResult> OnGet([FromQuery(Name = "RoomHash")] string hash)
        {
            string session = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(session))
            {
                ViewData["Username"] = "";
                return Redirect("/Login?Redirect=" + hash);
            }
            else
            {
                ViewData["Username"] = await ipp.GetUsername(session);
                User = await ipp.GetUser(session);
            }

            RoomHash = hash;
            Room = await ipp.GetRoom(hash);

            var d = new Code.Deathmatch(db);
            d.Join(hash, (int)User.Id);

            var domain = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Domain"];
            QrCode = Code.RoomManagement.GenerateQRCode(domain, hash);

            return Page();

        }

        public async Task<string> GetUsername(long id)
        {
            Models.User user = await ipp.GetUser(id);
            return user.Name;
        }
    }
}
