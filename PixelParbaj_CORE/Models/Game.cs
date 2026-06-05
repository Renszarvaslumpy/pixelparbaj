using System;
using System.Collections.Generic;

namespace PixelParbaj_CORE.Models
{
    public partial class Game
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public int? Movie1 { get; set; }
        public double? Time1 { get; set; }
        public int? Movie2 { get; set; }
        public double? Time2 { get; set; }
        public int? Movie3 { get; set; }
        public double? Time3 { get; set; }
        public int? Movie4 { get; set; }
        public double? Time4 { get; set; }
        public int? Movie5 { get; set; }
        public double? Time5 { get; set; }
        public int? Movie6 { get; set; }
        public double? Time6 { get; set; }
        public int? Movie7 { get; set; }
        public double? Time7 { get; set; }
        public int? Movie8 { get; set; }
        public double? Time8 { get; set; }
        public DateTime? Date { get; set; }
        public string? Room { get; set; }
        public int? Genre { get; set; }
        public int? Dif { get; set; }


        public double GetTime()
        {
            double all = Data.MovieSql.MOVIE_PER_GAME * 20.0;
            double result = (double)(all - (Time1 + Time2 + Time3 + Time4 + Time5 + Time6 + Time7 + Time8));
            return Math.Round(result, 3);
        }

        public int GetDifficulty()
        {
            switch (Dif)
            {
                case 0:
                default:
                    return 0;
                case 1:
                case 2:
                case 3:
                    return (int)Dif;
            }
        }

        public string GetDifficultyName()
        {
            switch (Dif)
            {
                case 0:
                default:
                    return "Normál";
                case 1:
                    return "Nehéz";
                case 2:
                    return "Mazochista";
                case 3:
                    return "Lehetetlen";
            }
        }

        public int GetHighlight()
        {
            double time = GetTime();
            if (time > 140)
            {
                return 1;
            }
            if (time > 120)
            {
                return 2;
            }
            if (time > 100)
            {
                return 3;
            }
            if (time > 80)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }

    }
}
