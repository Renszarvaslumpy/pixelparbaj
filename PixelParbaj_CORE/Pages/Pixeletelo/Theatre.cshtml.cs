using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PixelParbaj_CORE.Data;
using PixelParbaj_CORE.Models;

namespace PixelParbaj_CORE.Pages.Pixeletelo
{
    public class TheatreModel : PageModel
    {
        public List<Models.Movie> SelectedMovies;
        public string UserEmail { get; set;}
        public string RoomHash = "";
        public int Genre = 0;
        public int Dif = 0;

        public Data.IPP db { get; set; }
        public TheatreModel(Data.IPP db)
        {
            this.db = db;
        }

        public async Task<IActionResult> OnGet([FromQuery(Name = "RoomHash")] string hash)
        {
            RoomHash = hash;
            Room room = await db.GetRoom(hash);

            SelectedMovies = new List<Movie>() { };
            SelectedMovies.Add(await db.GetMovie((int)room.Movie1));
            SelectedMovies.Add(await db.GetMovie((int)room.Movie2));
            SelectedMovies.Add(await db.GetMovie((int)room.Movie3));
            SelectedMovies.Add(await db.GetMovie((int)room.Movie4));
            SelectedMovies.Add(await db.GetMovie((int)room.Movie5));
            SelectedMovies.Add(await db.GetMovie((int)room.Movie6));
            SelectedMovies.Add(await db.GetMovie((int)room.Movie7));
            SelectedMovies.Add(await db.GetMovie((int)room.Movie8));

            string session = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(session))
            {
                ViewData["Username"] = "";
            }
            else
            {
                ViewData["Username"] = await db.GetUsername(session);
                UserEmail = session;
            }

            return Page();
        }
    }
}
