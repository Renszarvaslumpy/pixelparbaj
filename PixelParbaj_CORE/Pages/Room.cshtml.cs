using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PixelParbaj_CORE.Pages
{
    public class RoomModel : PageModel
    {
        private Data.IPP ipp { get; set; }
        public string RoomHash { get; set; }
        public Models.User User { get; set; }
        public Models.Room Room { get; set; }
        public Models.Game YourGame { get; set; }
        public IEnumerable<Models.Game> Games { get; set; }

        public RoomModel(Data.IPP ipp)
        {
            this.ipp = ipp;
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

            YourGame = await ipp.GetGameForRoom((int)User.Id, hash);
            Games = await ipp.GetGamesForRoom(hash);

            if (Games != null)
            {
                Games = Games.OrderBy(x => x.GetTime());
            }

            return Page();

        }

        public async Task<string> GetUsername(long id)
        {
            Models.User user = await ipp.GetUser(id);
            return user.Name;
        }
    }
}
