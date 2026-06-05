using System;
using System.Collections.Generic;

namespace PixelParbaj_CORE.Models
{
    public partial class Room
    {
        public long Id { get; set; }
        public string? ShareId { get; set; }
        public string? RoomName { get; set; }
        public long? OwnerUserId { get; set; }
        public int? MaxUser { get; set; }
        public int? Movie1 { get; set; }
        public int? Movie2 { get; set; }
        public int? Movie3 { get; set; }
        public int? Movie4 { get; set; }
        public int? Movie5 { get; set; }
        public int? Movie6 { get; set; }
        public int? Movie7 { get; set; }
        public int? Movie8 { get; set; }
        public DateTime? Date { get; set; }
        public int? Genre { get; set; }
        public int? Dif { get; set; }

        public string GetName()
        {
            if (!string.IsNullOrEmpty(RoomName))
            {
                return RoomName;
            }

            if (MaxUser == 2)
            {
                return "Párbaj";
            }
            else
            {
                return "Parti!";
            }
        }
    }
}
