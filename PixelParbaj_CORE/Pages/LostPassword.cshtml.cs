using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE
{
    public class LostPasswordModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        public bool Filled { get; set; }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Models.User user = await ipp.GetUser(Email);

            if (user != null)
            {
                Filled = true;
                Code.UserManagement userManagement = new Code.UserManagement(Configuration);
                userManagement.SendResetEmail(user.Email, user.Password);
            }
            else
            {
                Filled = false;
            }

            return Page();
        }

        private Data.IPP ipp { get; set; }
        private readonly IConfiguration Configuration;
        public LostPasswordModel(Data.IPP ipp, IConfiguration configuration)
        {
            this.ipp = ipp;
            Configuration = configuration;
        }

    }
}
