using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE
{
    public class AboutModel : PageModel
    {
        public Models.User User { get; set; }

        public async Task<IActionResult> OnGet()
        {
            string session = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(session))
            {
                ViewData["Username"] = "";
            }
            else
            {
                string user = await ipp.GetUsername(session);
                ViewData["Username"] = user;
                User = await ipp.GetUser(session);
                ViewData["Avatar"] = await ipp.GetUserAvatar(User.Id);
            }

            return Page();
        }

        private Data.IPP ipp { get; set; }
        public AboutModel(Data.IPP ipp)
        {
            this.ipp = ipp;
        }

    }
}
