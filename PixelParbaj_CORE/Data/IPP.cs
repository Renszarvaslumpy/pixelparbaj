using static PixelParbaj_CORE.Data.MovieSql;

namespace PixelParbaj_CORE.Data
{
    public interface IPP
    {
        Task<IEnumerable<Models.Movie>> GetMovies();
        Task<Models.Movie> GetMovie(int ID);
        Task<IEnumerable<Models.Movie>> GetAllMovies();
        Task<IEnumerable<Models.Movie>> GetRandomMovies(Difficulty difficulty);
        Task<IEnumerable<SearchResult>> GetSearchedMovies(string Term);
        Task<IEnumerable<Models.Game>> GetAllGames();
        Task<IEnumerable<Models.Game>> GetLatestGames();
        Task<IEnumerable<Models.Game>> GetBestGames();
        Task<int> SaveGame(Models.Game game);
        Task<long> Registration(Models.User user);
        Task<IEnumerable<Models.User>> GetUsers();
        Task<int> Login(Models.User user);
        Task<int> ResetPassword(string email, string newPassword);
        Task<string> GetUsername(string email);
        Task<Models.User> GetUser(string email);
        Task<Models.User> GetUser(long id);
        Task<bool> UpdateUserPassword(long id, string password);
        Task<string> GetUserAvatar(long id);
        Task<bool> SetUserAvatar(long id, string filename);
        Task<int> CreateRoom(Models.Room room);
        Task<Models.Room> GetRoom(string roomHash);
        Task<Models.Game> GetGameForRoom(int userID, string roomHash);
        Task<IEnumerable<Models.Game>> GetGamesForUser(int userID);
        Task<bool> UserCanPlay(int userID);
        Task<IEnumerable<Models.Game>> GetGamesForRoom(string roomHash);
    }
}
