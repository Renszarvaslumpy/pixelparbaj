using System;
using System.Collections.Generic;

namespace PixelParbaj_CORE.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public byte[]? Avatar { get; set; }
        public int? Cheater {  get; set; }

        public static List<string> PredefinedAvatars = new List<string>()
        {
          "avatar_9.png",
            "avatar_8.png",
            "avatar_7.png",
            "avatar_6.png",
            "avatar_5.png",
            "avatar_4.png",
            "avatar_3.png",
            "avatar_2.png",
            "avatar_15.png",
            "avatar_16.png",
            "avatar_17.png",
            "avatar_18.png",
            "avatar_19.png",
            "avatar_20.png",
            "avatar_14.png",
            "avatar_13.png",
            "avatar_12.png",
            "avatar_11.png",
            "avatar_10.png",
            "avatar_1.png",
        };

        public void UpdateAvatar(string filename)
        {
            string fullpath = Path.Combine(Environment.CurrentDirectory, "wwwroot/gfx/avatars/" + filename);
            Avatar = File.ReadAllBytes(fullpath);
        }

        public string GetAvatar()
        {
            if (Avatar == null)
            {
                return null;
            }
            return Convert.ToBase64String(Avatar);
        }

        public static string GetAvatar(string filename)
        {
            string fullpath = Path.Combine(Environment.CurrentDirectory, "wwwroot/gfx/avatars/" + filename);
            byte[] avatar = File.ReadAllBytes(fullpath);
            return Convert.ToBase64String(avatar);
        }

        public string AssignRandomAvatar()
        {
            Random random = new Random();
            string filename = PredefinedAvatars[random.Next(0, PredefinedAvatars.Count)];
            UpdateAvatar(filename);
            return GetAvatar();
        }
    }

}
