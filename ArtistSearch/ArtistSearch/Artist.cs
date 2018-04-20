using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistSearch
{
    class Artist
    {
        public string Name { get; set; }
        public List<Album> Albums { get; set; }
    }
    class Album
    {
        public string AlbumTitle { get; set; }
        public List<string> Songs { get; set; }
    }
}
