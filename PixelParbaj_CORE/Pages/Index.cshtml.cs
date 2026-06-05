using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PixelParbaj_CORE.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Models.Game> latestGames;
        public List<Models.Game> bestGames;
        public Models.User User { get; set; }
        public string GithubRepo { get; set; }
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, Data.IPP ipp, IConfiguration configuration)
        {
            _logger = logger;
            this.ipp = ipp;
            _configuration = configuration;
        }
        private Data.IPP ipp { get; set; }

        public async Task<ActionResult> OnGet()
        {
            string session = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(session))
            {
                ViewData["Username"] = "";
            }
            else
            {
                string user = await ipp.GetUsername(session);
                ViewData["Username"] = user;
                User = await ipp.GetUser(session);
                ViewData["Avatar"] = await ipp.GetUserAvatar(User.Id);
            }

            var m = await ipp.GetLatestGames();
            latestGames = m.ToList();

            var m2 = await ipp.GetBestGames();
            bestGames = m2.ToList();

            GithubRepo = _configuration["AppSettings:GithubRepo"];

            return Page();
        }

        public async Task<string> GetUsername(long id)
        {
            Models.User user = await ipp.GetUser(id);
            if(user == null)
            {
                return "Anonymus";
            }
            return user.Name;
        }
        public async Task<string> GetUserAvatar(long id)
        {
            return await ipp.GetUserAvatar(id);
        }

    }
}