using System;
using System.Collections.Generic;
using System.Text;

namespace TwitRp.Core.Models.Emoji
{
    public class EmojiModel
    {
        public string name { get; set; }
        public string unified { get; set; }
        public string non_qualified { get; set; }
        public string docomo { get; set; }
        public string au { get; set; }
        public string softbank { get; set; }
        public string google { get; set; }
        public string image { get; set; }
        public int sheet_x { get; set; }
        public int sheet_y { get; set; }
        public string short_name { get; set; }
        public List<string> short_names { get; set; }
        public object text { get; set; }
        public object texts { get; set; }
        public string category { get; set; }
        public int sort_order { get; set; }
        public string added_in { get; set; }
        public bool has_img_apple { get; set; }
        public bool has_img_google { get; set; }
        public bool has_img_twitter { get; set; }
        public bool has_img_facebook { get; set; }
    }
}
