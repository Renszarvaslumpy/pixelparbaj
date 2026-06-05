using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE
{
    public class ProfileModel : PageModel
    {
        public Models.User user { get; set; }
        public int plays { get; set; }
        public async Task<IActionResult> OnGet()
        {
            string session = HttpContext.Session.GetString("Email");
            user = await ipp.GetUser(session);
            var games = await ipp.GetGamesForUser((int)user.Id);
            plays = games.Count();
            return Page();
        }

        public async Task<IActionResult> OnGetUpdate([FromQuery(Name = "Avatar")] string avatar)
        {
            string session = HttpContext.Session.GetString("Email");
            user = await ipp.GetUser(session);
            ipp.SetUserAvatar(user.Id, avatar);
            return Page();
        }

        private Data.IPP ipp { get; set; }
        public ProfileModel(Data.IPP ipp)
        {
            this.ipp = ipp;
        }

    }
}
