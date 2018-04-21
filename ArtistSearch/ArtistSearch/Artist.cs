using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace ArtistSearch
{
    class Artist
    {
        public string ArtistName { get; set; }
        public int ArtistID { get; set; }

        public List<Album> Albums { get; set; }

        public override string ToString()
        {
            return this.ArtistName;
        }
    }
    class Album
    {
        public string AlbumTitle { get; set; }
    }
}
