using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PixelParbaj_CORE.Models;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE.Pages
{
    public class SinglePlayerModel : PageModel
    {
        public List<Models.Movie> SelectedMovies;
        public List<SearchResult> SearchedMovies;
        public string UserEmail = "";
        public string RoomHash = "";
        public int Genre = 0;
        public int Dif = 0;

        private Data.IPP ipp { get; set; }
        public SinglePlayerModel(Data.IPP ipp)
        {
            this.ipp = ipp;
        }

        public async Task<IActionResult> OnGet([FromQuery(Name = "Genre")] int genre, [FromQuery(Name = "Dif")] int dif)
        {
            var m = await ipp.GetRandomMovies((Difficulty)dif);
            SelectedMovies = m.ToList();

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

            Genre = genre;
            Dif = dif;

            return Page();
        }

        public async Task<IActionResult> OnGetRoom([FromQuery(Name = "RoomHash")] string hash)
        {
            RoomHash = hash;

            Room room = await ipp.GetRoom(hash);
            Genre = (int)room.Genre;
            Dif = (int)room.Dif;

            SelectedMovies = new List<Movie>() { };
            SelectedMovies.Add(await ipp.GetMovie((int)room.Movie1));
            SelectedMovies.Add(await ipp.GetMovie((int)room.Movie2));
            SelectedMovies.Add(await ipp.GetMovie((int)room.Movie3));
            SelectedMovies.Add(await ipp.GetMovie((int)room.Movie4));
            SelectedMovies.Add(await ipp.GetMovie((int)room.Movie5));
            SelectedMovies.Add(await ipp.GetMovie((int)room.Movie6));
            SelectedMovies.Add(await ipp.GetMovie((int)room.Movie7));
            SelectedMovies.Add(await ipp.GetMovie((int)room.Movie8));

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
