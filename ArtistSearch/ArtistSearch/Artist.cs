using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Amazon.DynamoDBv2.DataModel;

namespace ArtistSearch
{
    internal class Artist
    {
        public string ArtistName { get; set; }
        public int ArtistId { get; set; }

        public List<Album> Albums { get; set; }

        public override string ToString()
        {
            return ArtistName;
        }

    }

    internal class Album
    {
        public string AlbumName { get; set; }
        public List<Song> Songs { get; set; }

        public override string ToString()
        {
            return AlbumName;
        }
    }

    internal class Song
    {
        public string SongName { get; set; }
        public override string ToString()
        {
            return SongName;
        }
    }
}
