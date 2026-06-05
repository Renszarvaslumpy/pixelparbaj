using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE
{
    public class RegistrationModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(Models.User user)
        {
            long result = await ipp.Registration(user);

            if (result == 0)
            {
                HttpContext.Session.SetString("Email", user.Email);
                return Redirect("/Index");
            }

            return Page();
        }

        private Data.IPP ipp { get; set; }
        public RegistrationModel(Data.IPP ipp)
        {
            this.ipp = ipp;
        }

    }
}
