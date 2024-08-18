using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class Song
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Duration { get; set; }
        public string ImageUrl { get; set; }

        public double DurationInSeconds
        {
            get
            {
                var parts = Duration.Split(':');
                if (parts.Length == 2)
                {
                    if (int.TryParse(parts[0], out int minutes) && int.TryParse(parts[1], out int seconds))
                    {
                        return (minutes * 60) + seconds;
                    }
                }
                return 0; // Default value if conversion fails
            }
        }

        public string AudioUrl { get; set; }
        public string ExternalUrl { get; set; }
    }
}
