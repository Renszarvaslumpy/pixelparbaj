using PixelParbaj_CORE.Models;

namespace PixelParbaj_CORE.Code
{
    public class Deathmatch
    {
        public PP2Context db { get; set; }
        public Deathmatch(PP2Context db)
        {
            this.db = db;
        }

        public bool Join(string RoomHash, int UserId)
        {
            if (!db.Joins.Any(x => x.RoomHash == RoomHash && x.UserId == UserId))
            {
                var join = new Join();
                join.RoomHash = RoomHash;
                join.UserId = UserId;
                db.Joins.Add(join);
                db.SaveChanges();
            }
            return true;
        }

        public List<Join> GetJoins(string RoomHash)
        {
            if (db.Joins.Any(x => x.RoomHash == RoomHash))
            {
                return db.Joins.Where(x => x.RoomHash == RoomHash).ToList();
            }
            return null;
        }

        public bool StartTheatre(string RoomHash)
        {
            if (db.Scenes.Any(x => x.RoomHash == RoomHash))
            {
                var scene = db.Scenes.Where(x => x.RoomHash == RoomHash).FirstOrDefault();
                scene.Started = 1;
                scene.Scene1Started = DateTime.Now;
                db.SaveChanges();
            }
            else
            {
                var scene = new Scene();
                scene.RoomHash = RoomHash;
                scene.Started = 1;
                scene.Scene1Started = DateTime.Now;
                db.Scenes.Add(scene);
                db.SaveChanges();
            }
            return true;
        }

        public bool StartScene(string RoomHash, int Movie)
        {
            var scene = db.Scenes.Where(x => x.RoomHash == RoomHash).FirstOrDefault();

            Movie = Movie + 1;
            switch (Movie)
            {
                case 1:
                    scene.Scene2Started = DateTime.Now;
                    break;
                case 2:
                    scene.Scene3Started = DateTime.Now;
                    break;
                case 3:
                    scene.Scene4Started = DateTime.Now;
                    break;
                case 4:
                    scene.Scene5Started = DateTime.Now;
                    break;
                case 5:
                    scene.Scene6Started = DateTime.Now;
                    break;
                case 6:
                    scene.Scene7Started = DateTime.Now;
                    break;
                case 7:
                    scene.Scene8Started = DateTime.Now;
                    break;
            }
            db.SaveChanges();
            return true;
        }

        public bool UpdateTheatre(string RoomHash, int Movie)
        {
            Scene scene = new Scene();
            if (db.Scenes.Any(x => x.RoomHash == RoomHash))
            {
                scene = db.Scenes.Where(x => x.RoomHash == RoomHash).FirstOrDefault();
            }
            else
            {
                scene = new Scene();
                scene.RoomHash = RoomHash;
                db.Scenes.Add(scene);
            }

            var room = db.Rooms.Where(x => x.ShareId == RoomHash).FirstOrDefault();
            var games = db.Games.Where(x => x.Room == RoomHash).ToList();
            var joins = db.Joins.Where(x => x.RoomHash == RoomHash).ToList();

            foreach (var j in joins)
            {
                if (!games.Any(x => x.UserId == j.UserId))
                {
                    var game = new Game();
                    game.UserId = j.UserId;
                    game.Room = RoomHash;
                    game.Genre = room.Genre;
                    game.Dif = room.Dif;                    
                    db.Games.Add(game);
                    db.SaveChanges();
                    games.Add(game);
                }
            }

            Movie = Movie + 1;
            switch (Movie)
            {
                case 1:
                    scene.Scene1 = 1;

                    foreach (var g in games)
                    {
                        if (g.Time1 == null)
                        {
                            g.Movie1 = room.Movie1;
                            g.Time1 = 0;
                        }
                    }

                    break;
                case 2:
                    scene.Scene2 = 1;
                    foreach (var g in games)
                    {
                        if (g.Time2 == null)
                        {
                            g.Movie2 = room.Movie2;
                            g.Time2 = 0;
                        }
                    }
                    break;
                case 3:
                    scene.Scene3 = 1;
                    foreach (var g in games)
                    {
                        if (g.Time3 == null)
                        {
                            g.Movie3 = room.Movie3;
                            g.Time3 = 0;
                        }
                    }
                    break;
                case 4:
                    scene.Scene4 = 1;
                    foreach (var g in games)
                    {
                        if (g.Time4 == null)
                        {
                            g.Movie4 = room.Movie4;
                            g.Time4 = 0;
                        }
                    }
                    break;
                case 5:
                    scene.Scene5 = 1;
                    foreach (var g in games)
                    {
                        if (g.Time5 == null)
                        {
                            g.Movie5 = room.Movie5;
                            g.Time5 = 0;
                        }
                    }
                    break;
                case 6:
                    scene.Scene6 = 1;
                    foreach (var g in games)
                    {
                        if (g.Time6 == null)
                        {
                            g.Movie6 = room.Movie6;
                            g.Time6 = 0;
                        }
                    }
                    break;
                case 7:
                    scene.Scene7 = 1;
                    foreach (var g in games)
                    {
                        if (g.Time7 == null)
                        {
                            g.Movie7 = room.Movie7;
                            g.Time7 = 0;
                        }
                    }
                    break;
                case 8:
                    scene.Scene8 = 1;
                    foreach (var g in games)
                    {
                        if (g.Time8 == null)
                        {
                            g.Movie8 = room.Movie8;
                            g.Time8 = 0;
                        }
                    }
                    scene.Finished = 1;
                    break;
            }
            db.SaveChanges();
            return true;
        }

        public List<Game> GetProgress(string RoomHash)
        {
            if (db.Games.Any(x => x.Room == RoomHash))
            {
                return db.Games.Where(x => x.Room == RoomHash).ToList();
            }
            return null;
        }

        public Scene GetScene(string RoomHash)
        {
            if (db.Scenes.Any(x => x.RoomHash == RoomHash))
            {
                return db.Scenes.Where(x => x.RoomHash == RoomHash).FirstOrDefault();
            }
            return null;
        }

        public bool UpdateTheatreProgress(long UserId, string RoomHash, int Movie)
        {
            var scene = db.Scenes.Where(x => x.RoomHash == RoomHash).FirstOrDefault();
            var room = db.Rooms.Where(x => x.ShareId == RoomHash).FirstOrDefault();

            Game game = new Game();
            if (db.Games.Any(x => x.Room == RoomHash && x.UserId == UserId))
            {
                game = db.Games.Where(x => x.Room == RoomHash && x.UserId == UserId).FirstOrDefault();
            }
            else
            {
                game = new Game();
                game.UserId = (int)UserId;
                game.Room = RoomHash;
                game.Genre = room.Genre;
                game.Dif = room.Dif;
                db.Games.Add(game);
            }

            switch (Movie)
            {
                case 1:
                    game.Movie1 = room.Movie1;
                    game.Time1 = (DateTime.Now - (DateTime)scene.Scene1Started).TotalSeconds;
                    game.Time1 = 20.0 - Math.Round((double)game.Time1, 3);
                    break;
                case 2:
                    game.Movie2 = room.Movie2;
                    game.Time2 = (DateTime.Now - (DateTime)scene.Scene2Started).TotalSeconds;
                    game.Time2 = 20.0 - Math.Round((double)game.Time2, 3);
                    break;
                case 3:
                    game.Movie3 = room.Movie3;
                    game.Time3 = (DateTime.Now - (DateTime)scene.Scene3Started).TotalSeconds;
                    game.Time3 = 20.0 - Math.Round((double)game.Time3, 3);
                    break;
                case 4:
                    game.Movie4 = room.Movie4;
                    game.Time4 = (DateTime.Now - (DateTime)scene.Scene4Started).TotalSeconds;
                    game.Time4 = 20.0 - Math.Round((double)game.Time4, 3);
                    break;
                case 5:
                    game.Movie5 = room.Movie5;
                    game.Time5 = (DateTime.Now - (DateTime)scene.Scene5Started).TotalSeconds;
                    game.Time5 = 20.0 - Math.Round((double)game.Time5, 3);
                    break;
                case 6:
                    game.Movie6 = room.Movie6;
                    game.Time6 = (DateTime.Now - (DateTime)scene.Scene6Started).TotalSeconds;
                    game.Time6 = 20.0 - Math.Round((double)game.Time6, 3);
                    break;
                case 7:
                    game.Movie7 = room.Movie7;
                    game.Time7 = (DateTime.Now - (DateTime)scene.Scene7Started).TotalSeconds;
                    game.Time7 = 20.0 - Math.Round((double)game.Time7, 3);
                    break;
                case 8:
                    game.Movie8 = room.Movie8;
                    game.Time8 = (DateTime.Now - (DateTime)scene.Scene8Started).TotalSeconds;
                    game.Time8 = 20.0 - Math.Round((double)game.Time8, 3);
                    game.Date = DateTime.Now;
                    break;
            }
            db.SaveChanges();
            return true;
        }

        public List<Game> GetFinishedGames(string roomHash)
        {
            double all = Data.MovieSql.MOVIE_PER_GAME * 20.0;
            return db.Games.Where(x => x.Room == roomHash).OrderBy(x => (all - (x.Time1 + x.Time2 + x.Time3 + x.Time4 + x.Time5 + x.Time6 + x.Time7 + x.Time8))).ToList();
        }

    }
}
