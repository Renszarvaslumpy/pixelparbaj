using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PixelParbaj_CORE.Models;
using System.Security.Policy;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE.Pages
{
    public class ClientModel : PageModel
    {
        public Room Room;
        public string UserEmail = "";
        public string RoomHash = "";
        public int Genre = 0;
        public int Dif = 0;

        private Data.IPP ipp { get; set; }
        public ClientModel(Data.IPP ipp)
        {
            this.ipp = ipp;
        }

        public async Task<IActionResult> OnGet(string RoomHash)
        {
            Room = await ipp.GetRoom(RoomHash);
            this.RoomHash = RoomHash;

            string session = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(session))
            {
                ViewData["Username"] = "";
            }
            else
            {
                ViewData["Username"] = await ipp.GetUsername(session);
                UserEmail = session;
            }

            return Page();
        }

    }
}
