using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLib
{
    public class PhotoItem
    {
        public long IdFromPlat { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Removed { get; set; } = false;
    }
}
