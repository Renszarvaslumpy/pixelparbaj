using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [HiddenInput]
        //I also tried to add [BindProperty] after
        public string submitRedirect { get; set; }

        public int Login { get; set; } = 0;

        public string RedirectURL { get; set; }
        public async Task<IActionResult> OnGet([FromQuery(Name = "Redirect")] string redirect)
        {
            if (!string.IsNullOrEmpty(redirect))
            {
                RedirectURL = redirect;
            }
            else
            {
                RedirectURL = "";
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(Models.User user)
        {
            Login = await ipp.Login(user);
            RedirectURL = submitRedirect;

            if (Login == 0)
            {
                HttpContext.Session.SetString("Email", user.Email);
                if (string.IsNullOrEmpty(RedirectURL))
                {
                    return Redirect("/Index");
                }
                else
                {
                    return Redirect("/Room?RoomHash=" + RedirectURL);
                }
                
            }
            return Page();

        }

        private Data.IPP ipp { get; set; }
        public LoginModel(Data.IPP ipp)
        {
            this.ipp = ipp;
        }

    }
}
