using Microsoft.AspNetCore.Mvc;
using PixelParbaj_CORE.Code;
using PixelParbaj_CORE.Data;
using PixelParbaj_CORE.Models;

namespace PixelParbaj_CORE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("[controller]")]
    public class PixeleteloController : ControllerBase
    {
        private readonly PP2Context db;
        private readonly IPP ipp;

        public PixeleteloController(PP2Context db, IPP ipp)
        {
            this.db = db;
            this.ipp = ipp;
        }

        [HttpGet("Get")]
        public IActionResult Get([FromQuery] string RoomHash)
        {
            var d = new Deathmatch(db);
            var joins = d.GetJoins(RoomHash) ?? new List<Join>();
            var ids = joins.Select(x => x.UserId).ToList();
            var players = db.Users.Where(x => ids.Contains((int)x.Id)).ToList();

            return Ok(new
            {
                joins = players
            });
        }

        [HttpGet("GetProgress")]
        public IActionResult GetProgress([FromQuery] string RoomHash, [FromQuery] int Scene)
        {
            var players = new List<User?>();
            var d = new Deathmatch(db);
            var progress = d.GetProgress(RoomHash);

            if (progress != null)
            {
                progress = Scene switch
                {
                    0 => progress.OrderByDescending(x => x.Time1).ToList(),
                    1 => progress.OrderByDescending(x => x.Time2).ToList(),
                    2 => progress.OrderByDescending(x => x.Time3).ToList(),
                    3 => progress.OrderByDescending(x => x.Time4).ToList(),
                    4 => progress.OrderByDescending(x => x.Time5).ToList(),
                    5 => progress.OrderByDescending(x => x.Time6).ToList(),
                    6 => progress.OrderByDescending(x => x.Time7).ToList(),
                    7 => progress.OrderByDescending(x => x.Time8).ToList(),
                    _ => progress
                };

                var ids = progress.Select(x => x.UserId).ToList();
                foreach (var id in ids)
                {
                    players.Add(db.Users.FirstOrDefault(x => x.Id == id));
                }
            }

            return Ok(new
            {
                sceneProgress = progress,
                scenePlayers = players
            });
        }

        [HttpGet("GetResults")]
        public IActionResult GetResults([FromQuery] string RoomHash)
        {
            var d = new Deathmatch(db);
            var games = d.GetFinishedGames(RoomHash);
            var results = new List<double>();
            var players = new List<User?>();

            foreach (var game in games)
            {
                results.Add(game.GetTime());
                players.Add(db.Users.Find(game.UserId));
            }

            return Ok(new
            {
                finishedGames = games,
                finishedTime = results,
                finishedPlayers = players
            });
        }

        [HttpGet("GetScene")]
        public IActionResult GetScene([FromQuery] string RoomHash)
        {
            var d = new Deathmatch(db);

            return Ok(new
            {
                scene = d.GetScene(RoomHash)
            });
        }

        [HttpGet("Join")]
        public IActionResult Join([FromQuery] string RoomHash, [FromQuery] int UserId)
        {
            if (!db.Rooms.Any(x => x.ShareId == RoomHash))
            {
                return Ok(new
                {
                    JoinSuccess = false
                });
            }

            var d = new Deathmatch(db);

            return Ok(new
            {
                JoinSuccess = d.Join(RoomHash, UserId)
            });
        }

        [HttpGet("SmartJoin")]
        public async Task<IActionResult> SmartJoin([FromQuery] string RoomHash)
        {
            var d = new Deathmatch(db);
            long userId = 0;
            string? session = HttpContext.Session.GetString("Email");

            if (!string.IsNullOrEmpty(session))
            {
                var user = await ipp.GetUser(session);
                userId = user.Id;
            }

            if (userId == 0)
            {
                var user = UserManagement.GenerateNewAnonymousUser();
                var userSuccess = await ipp.Registration(user);
                if (userSuccess == 0)
                {
                    HttpContext.Session.SetString("Email", user.Email);
                    userId = user.Id;
                }
                else
                {
                    return Redirect("/Duel?Type=2");
                }
            }

            var success = d.Join(RoomHash, Convert.ToInt32(userId));
            return success
                ? Redirect("/Pixeletelo/Client?RoomHash=" + RoomHash)
                : Redirect("/Duel?Type=2");
        }

        [HttpGet("Start")]
        public IActionResult Start([FromQuery] string RoomHash)
        {
            var d = new Deathmatch(db);
            return Ok(new { success = d.StartTheatre(RoomHash) });
        }

        [HttpGet("StartScene")]
        public IActionResult StartScene([FromQuery] string RoomHash, [FromQuery] int Movie)
        {
            var d = new Deathmatch(db);
            return Ok(new { success = d.StartScene(RoomHash, Movie) });
        }

        [HttpGet("Update")]
        public IActionResult Update([FromQuery] string RoomHash, [FromQuery] int Movie)
        {
            var d = new Deathmatch(db);
            return Ok(new { success = d.UpdateTheatre(RoomHash, Movie) });
        }

        [HttpGet("UpdateClientProgress")]
        public IActionResult UpdateClientProgress([FromQuery] string RoomHash, [FromQuery] int Movie)
        {
            string? session = HttpContext.Session.GetString("Email");
            var user = db.Users.FirstOrDefault(x => x.Email == session);
            if (user == null)
            {
                return BadRequest(new { success = false });
            }

            var d = new Deathmatch(db);
            return Ok(new { success = d.UpdateTheatreProgress(user.Id, RoomHash, Movie) });
        }
    }
}
