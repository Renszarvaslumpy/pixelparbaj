using System;
using System.Collections.Generic;

namespace PixelParbaj_CORE.Models
{
    public partial class Join
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public string? RoomHash { get; set; }

    }
}
