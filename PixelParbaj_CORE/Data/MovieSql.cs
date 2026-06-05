using Microsoft.EntityFrameworkCore;
using PixelParbaj_CORE.Models;

namespace PixelParbaj_CORE.Data
{
    public class MovieSql : IPP
    {
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard,
            Impossible
        }

        public class SearchResult
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int Year { get; set; }
        }

        private PP2Context PP2Context { get; set; }
        public static int MOVIE_PER_GAME = 8;
        public MovieSql(PP2Context context)
        {
            PP2Context = context;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await PP2Context.Movies.Take(8).ToListAsync();
        }


        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return await PP2Context.Movies.ToListAsync();
        }

        public async Task<IEnumerable<SearchResult>> GetSearchedMovies(string term)
        {
            List<SearchResult> results = new List<SearchResult>();
            
            // method 1
            await PP2Context.Movies.Where(x => x.TitleH.Contains(term)).Take(30).ForEachAsync(x => results.Add(new SearchResult { Id = x.Id, Name = x.TitleH + " (" + (x.Year) + ")" }));
            await PP2Context.Movies.Where(x => !string.IsNullOrEmpty(x.TitleE) && x.TitleE.Contains(term)).Take(30).ForEachAsync(x => results.Add(new SearchResult { Id = x.Id, Name = x.TitleE + " (" + (x.Year) + ")" }));
            
            // method 2
            /*foreach(var Movie in await PP2Context.Movies.Where(x => x.TitleH.Contains(term)).ToListAsync())
            {
                results.Add(new SearchResult { Id = Movie.Id, Name = Movie.TitleH + " (" + (Movie.Year) + ")" });
            }
            foreach (var Movie in await PP2Context.Movies.Where(x => !string.IsNullOrEmpty(x.TitleE) && x.TitleE.Contains(term)).ToListAsync())
            {
                results.Add(new SearchResult { Id = Movie.Id, Name = Movie.TitleE + " (" + (Movie.Year) + ")" });
            }*/
            return results.Take(30);
        }

        public async Task<IEnumerable<Movie>> GetRandomMovies(Difficulty difficulty)
        {
            int allMovies;
            switch (difficulty)
            {
                case Difficulty.Easy:
                default:
                    allMovies = 500;
                    break;
                case Difficulty.Medium:
                    allMovies = 1000;
                    break;
                case Difficulty.Hard:
                    allMovies = 1500;
                    break;
                case Difficulty.Impossible:
                    allMovies = PP2Context.Movies.Count();
                    break;
            }
            
            List<Movie> selectedMovies = new List<Movie>() { };
            List<int> indexes = new List<int>() { };

            var movieRage = PP2Context.Movies.OrderByDescending(x => x.VotesInt).Take(allMovies);

            Random random = new Random();
            for(int i = 0; i < MOVIE_PER_GAME; i++)
            {
                int index = random.Next(0, allMovies);
                while (indexes.Contains(index))
                {
                    index = random.Next(0, allMovies);
                }

                var movie = movieRage.Skip(index).First();
                selectedMovies.Add(movie);
                indexes.Add(index);
            }
            return selectedMovies;
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await PP2Context.Games.ToListAsync();
        }
        public async Task<int> SaveGame(Game game)
        {
            await PP2Context.Games.AddAsync(game);
            PP2Context.SaveChanges();
            return 0;
        }

        public async Task<long> Registration(User user)
        {
            if (PP2Context.Users.Any(x => x.Email == user.Email))
            {
                return -1;
            }
            if(string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return -2;
            }
            user.Password = Code.UserManagement.Encrypt(user.Password); // encryption
            user.AssignRandomAvatar();
            await PP2Context.Users.AddAsync(user);
            PP2Context.SaveChanges();
            return 0;
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Login(User user)
        {
            if (!PP2Context.Users.Any(x => x.Email == user.Email))
            {
                return -1;
            }
            if (Code.UserManagement.Encrypt(user.Password) == PP2Context.Users.First(x => x.Email == user.Email).Password)
            {
                return 0;
            }
            return -2;
        }

        public async Task<string> GetUsername(string email)
        {
            if (!PP2Context.Users.Any(x => x.Email == email))
            {
                return "";
            }
            return PP2Context.Users.First(x => x.Email == email).Name;
        }

        public async Task<User> GetUser(string email)
        {
            if (!PP2Context.Users.Any(x => x.Email == email))
            {
                return null;
            }
            return PP2Context.Users.First(x => x.Email == email);
        }

        public async Task<User> GetUser(long id)
        {
            if (!PP2Context.Users.Any(x => x.Id == id))
            {
                return null;
            }
            return PP2Context.Users.First(x => x.Id == id);
        }

        public async Task<int> CreateRoom(Room room)
        {
            await PP2Context.Rooms.AddAsync(room);
            PP2Context.SaveChanges();
            return 0;
        }

        public async Task<Room> GetRoom(string hash)
        {
            if (!PP2Context.Rooms.Any(x => x.ShareId == hash))
            {
                return null;
            }
            return PP2Context.Rooms.First(x => x.ShareId == hash);
        }

        public async Task<Game> GetGameForRoom(int userID, string roomHash)
        {
            if (PP2Context.Games.Any(x => x.UserId == userID && x.Room == roomHash))
            {
                return await PP2Context.Games.FirstAsync(x => x.UserId == userID && x.Room == roomHash);
            }
            return null;
        }

        public async Task<IEnumerable<Game>> GetGamesForRoom(string roomHash)
        {
            if (PP2Context.Games.Any(x => x.Room == roomHash))
            {
                return await PP2Context.Games.Where(x => x.Room == roomHash).ToListAsync();
            }
            return null;
        }

        public async Task<Movie> GetMovie(int ID)
        {
            if (PP2Context.Movies.Any(x => x.Id == ID))
            {
                return await PP2Context.Movies.FirstAsync(x => x.Id == ID);
            }
            return null;
        }

        public async Task<User> GetUser(int id)
        {
            return PP2Context.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Game>> GetLatestGames()
        {
            var games = await PP2Context.Games.OrderByDescending(x => x.Date).Take(50).ToListAsync();
            
            List<Game> latestGames = new List<Game>();
            foreach (var game in games)
            {
                if (game.UserId == null)
                {
                    continue;
                }
                var user = await GetUser((int)game.UserId);
                if (user != null && user.Cheater == 0)
                {
                    latestGames.Add(game);
                }
                if (latestGames.Count == 10)
                {
                    break;
                }
            }
            return latestGames;
        }

        public async Task<IEnumerable<Game>> GetBestGames()
        {
            double all = Data.MovieSql.MOVIE_PER_GAME * 20.0;
            var games = await PP2Context.Games.OrderBy(x => (all - (x.Time1 + x.Time2 + x.Time3 + x.Time4 + x.Time5 + x.Time6 + x.Time7 + x.Time8))).Take(50).ToListAsync();
            List<Game> bestGames = new List<Game>();
            foreach (var game in games)
            {
                if (game.UserId == null)
                {
                    continue;
                }
                var user = await GetUser((int)game.UserId);
                if (user != null && user.Cheater == 0)
                {
                    bestGames.Add(game);
                }
                if (bestGames.Count == 10)
                {
                    break;
                }
            }
            return bestGames;
        }

        public async Task<int> ResetPassword(string email, string newPassword)
        {
            User user = await PP2Context.Users.FirstAsync(x => x.Email == email);
            user.Password = Code.UserManagement.Encrypt(newPassword);
            await PP2Context.SaveChangesAsync();
            return 0;
        }

        public async Task<bool> UpdateUserPassword(long id, string password)
        {
            if (!PP2Context.Users.Any(x => x.Id == id))
            {
                return false;
            }
            Models.User user = PP2Context.Users.First(x => x.Id == id);
            user.Password = password;
            PP2Context.SaveChanges();
            return true;
        }

        public async Task<string> GetUserAvatar(long id)
        {
            if (!PP2Context.Users.Any(x => x.Id == id))
            {
                return null;
            }
            User user = PP2Context.Users.First(x => x.Id == id);

            string avatar = user.GetAvatar();

            if (avatar == null)
            {
                avatar = user.AssignRandomAvatar();
                PP2Context.SaveChanges();
            }
            return avatar;

        }

        public async Task<bool> SetUserAvatar(long id, string filename)
        {
            if (!PP2Context.Users.Any(x => x.Id == id))
            {
                return false;
            }
            User user = PP2Context.Users.First(x => x.Id == id);
            user.UpdateAvatar(filename);
            PP2Context.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<Game>> GetGamesForUser(int userID)
        {
            if (PP2Context.Games.Any(x => x.UserId == userID))
            {
                return await PP2Context.Games.Where(x => x.UserId == userID).ToListAsync();
            }
            return null;
        }

        public async Task<bool> UserCanPlay(int userID)
        {
            if (PP2Context.Games.Any(x => x.UserId == userID))
            {
                // games today
                int plays = PP2Context.Games.Count(x => x.UserId == userID && x.Date.Value.Date == DateTime.Now.Date);
                return plays < 3;
            }
            return true;
        }
    }
}
