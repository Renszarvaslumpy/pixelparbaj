using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        [HiddenInput]
        //I also tried to add [BindProperty] after
        public string OldKey { get; set; }
        public string Email { get; set; }
        public async Task<IActionResult> OnGet([FromQuery(Name = "key")] string key, [FromQuery(Name = "email")] string email)
        {
            OldKey = key;
            Email = email;

            return Page();
        }

        public async Task<IActionResult> OnPost(Models.User user)
        {
            Models.User u = await ipp.GetUser(user.Email);
            string oldKey = OldKey;

            if (oldKey == u.Password.Replace('+',' '))
            {
                await ipp.UpdateUserPassword(u.Id, Code.UserManagement.Encrypt(user.Password));
                return Redirect("/Login");
            }
            return Page();

        }

        private Data.IPP ipp { get; set; }
        public ResetPasswordModel(Data.IPP ipp)
        {
            this.ipp = ipp;
        }

    }
}
