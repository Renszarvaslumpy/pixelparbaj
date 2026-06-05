using Microsoft.AspNetCore.Mvc;
using PixelParbaj_CORE.Models;

namespace PixelParbaj_CORE.Controllers
{
    [ApiController]
    [Route("api/maintenance")]
    public class MaintenanceController : ControllerBase
    {
        private readonly PP2Context db;

        public MaintenanceController(PP2Context db)
        {
            this.db = db;
        }

        [HttpGet("UpdateVotes")]
        [HttpGet("/API/Maintenance/UpdateVotes")]
        public IActionResult UpdateVotes()
        {
            foreach (var movie in db.Movies)
            {
                var vote = movie.Votes.Replace("K", "000").Replace(".", string.Empty);
                movie.VotesInt = int.Parse(vote);
            }

            db.SaveChanges();
            return Ok(new { success = true });
        }
    }
}
