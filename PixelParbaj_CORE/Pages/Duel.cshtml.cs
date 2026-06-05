using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static PixelParbaj_CORE.Code.RoomManagement;

namespace PixelParbaj_CORE.Pages
{
    public class DuelModel : PageModel
    {
        private Data.IPP ipp { get; set; }
        public Models.User User;
        public long UserId { get; set; }
        public string UserEmail { get; set; }
        public RoomType RoomType { get; set; }
        public int Max { get; set; }

        public DuelModel(Data.IPP ipp)
        {
            this.ipp = ipp;
        }

        public async Task<IActionResult> OnGet([FromQuery(Name = "Type")] string type)
        {
            UserEmail = "";
            UserId = 0;
            string session = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(session))
            {
                ViewData["Username"] = "";
            }
            else
            {
                ViewData["Username"] = await ipp.GetUsername(session);
                User = await ipp.GetUser(session);
                UserEmail = User.Email;
                UserId = User.Id;
            }

            switch (type)
            {
                case "0":
                    RoomType = RoomType.Duel;
                    Max = 2;
                    break;
                case "1":
                    RoomType = RoomType.Party;
                    Max = 100;
                    break;
                case "2":
                    RoomType = RoomType.Deathmatch;
                    Max = 999;
                    break;
            }
            
            return Page();

        }
    }
}
