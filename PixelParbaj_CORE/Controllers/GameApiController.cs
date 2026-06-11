using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PixelParbaj_CORE.Data;
using PixelParbaj_CORE.Models;
using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE.Controllers
{
    [ApiController]
    [Route("api")]
    public class GameApiController : ControllerBase
    {
        private readonly IPP ipp;

        public GameApiController(IPP ipp)
        {
            this.ipp = ipp;
        }

        [HttpGet("ActiveSearch")]
        [HttpGet("/ActiveSearch")]
        public async Task<IActionResult> ActiveSearch([FromQuery] string Term)
        {
            var movies = await ipp.GetSearchedMovies(Term);
            return Ok(movies.ToList());
        }

        [HttpGet("GenerateRoom")]
        [HttpGet("/GenerateRoom")]
        public async Task<IActionResult> GenerateRoom(
            [FromQuery] int Dif,
            [FromQuery] int Genre,
            [FromQuery] string Name,
            [FromQuery] string Owner,
            [FromQuery] int Max)
        {
            User user = await ipp.GetUser(Owner);

            Room room = new Room
            {
                ShareId = Code.RoomManagement.GenerateID(),
                OwnerUserId = user.Id,
                MaxUser = Max,
                RoomName = Name,
                Dif = Dif,
                Genre = Genre,
                Date = DateTime.Now
            };

            var movies = (await ipp.GetRandomMovies((Difficulty)Dif)).ToList();
            room.Movie1 = (int)movies[0].Id;
            room.Movie2 = (int)movies[1].Id;
            room.Movie3 = (int)movies[2].Id;
            room.Movie4 = (int)movies[3].Id;
            room.Movie5 = (int)movies[4].Id;
            room.Movie6 = (int)movies[5].Id;
            room.Movie7 = (int)movies[6].Id;
            room.Movie8 = (int)movies[7].Id;

            await ipp.CreateRoom(room);
            return Ok(room.ShareId);
        }

        [HttpGet("SaveGame")]
        [HttpGet("/SaveGame")]
        public async Task<IActionResult> SaveGame(
            [FromQuery] int M1,
            [FromQuery] int M2,
            [FromQuery] int M3,
            [FromQuery] int M4,
            [FromQuery] int M5,
            [FromQuery] int M6,
            [FromQuery] int M7,
            [FromQuery] int M8,
            [FromQuery] string T1,
            [FromQuery] string T2,
            [FromQuery] string T3,
            [FromQuery] string T4,
            [FromQuery] string T5,
            [FromQuery] string T6,
            [FromQuery] string T7,
            [FromQuery] string T8,
            [FromQuery] string? Email,
            [FromQuery] string? Room,
            [FromQuery] int Genre,
            [FromQuery] int Dif)
        {
            Game game = new Game
            {
                Movie1 = M1,
                Movie2 = M2,
                Movie3 = M3,
                Movie4 = M4,
                Movie5 = M5,
                Movie6 = M6,
                Movie7 = M7,
                Movie8 = M8,
                Time1 = ParseGameTime(T1),
                Time2 = ParseGameTime(T2),
                Time3 = ParseGameTime(T3),
                Time4 = ParseGameTime(T4),
                Time5 = ParseGameTime(T5),
                Time6 = ParseGameTime(T6),
                Time7 = ParseGameTime(T7),
                Time8 = ParseGameTime(T8),
                Date = DateTime.Now,
                Room = Room
            };

            if (string.IsNullOrEmpty(Email))
            {
                game.UserId = 0;
            }
            else
            {
                User user = await ipp.GetUser(Email);
                if (user != null)
                {
                    game.UserId = user.Id;
                }
            }

            if (!string.IsNullOrEmpty(Room))
            {
                Room dbRoom = await ipp.GetRoom(Room);
                game.Genre = dbRoom.Genre;
                game.Dif = dbRoom.Dif;
            }
            else
            {
                game.Genre = Genre;
                game.Dif = Dif;
            }

            int result = await ipp.SaveGame(game);
            return Ok(result);
        }

        private static double ParseGameTime(string time)
        {
            if (double.TryParse(time, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double parsedTime))
            {
                return parsedTime;
            }

            return double.Parse(time, CultureInfo.CurrentCulture);
        }
    }
}
