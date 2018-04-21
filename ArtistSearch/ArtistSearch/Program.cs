using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace ArtistSearch
{
    internal class Program
    {
        private static readonly AmazonDynamoDBClient Client = new AmazonDynamoDBClient(new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.EUWest1 });

        public List<Artist> DbGetArtists(string a)
        {
            try
            {
                //Artist name to lower
                string artist = a.ToLower();

                //Load artists table
                var table = Table.LoadTable(Client, "Artists");

                //Filter for scan. Artist column matches artist input
                ScanFilter filter = new ScanFilter();
                filter.AddCondition("ArtistName", ScanOperator.Contains, artist);

                //Response
                List<Document> response = table.Scan(filter).GetNextSet();

                //List to contain found artists
                List<Artist> foundArtists = new List<Artist>();

                //Loop through each item in the response and map it to a new Artist in the artist list
                foreach (Document item in response)
                {
                    foundArtists.Add(new Artist() { ArtistName = item["ArtistName"], ArtistID = int.Parse(item["ArtistID"]) });
                }

                return foundArtists;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public List<Album> DbGetAlbums(Artist artist)
        {
            try
            {
                //Artist name + id = key to table
                string artistNameID = (artist.ArtistName + " " + artist.ArtistID);

                Console.WriteLine(artistNameID);

                //Load albums table
                var table = Table.LoadTable(Client, "Albums");

                //Filter for query. Artist column matches artist input
                QueryFilter filter = new QueryFilter();
                filter.AddCondition("ArtistNameID", QueryOperator.Equal, artistNameID);

                //Response
                List<Document> response = table.Query(filter).GetNextSet();

                //List to contain found artists
                List<Album> foundAlbums = new List<Album>();

                //Loop through each item in the response and map it to a new Artist in the artist list
                foreach (Document item in response)
                {
                    foundAlbums.Add(new Album() { AlbumName = item["AlbumName"] });
                }

                return foundAlbums;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /*public void DbGetAlbums(string artist)
        {
            Console.WriteLine(artist);
            var table = Table.LoadTable(Client, "Albums");
            QueryFilter filter = new QueryFilter();
            filter.AddCondition("Artist", QueryOperator.Equal, artist);
            var response = table.Query(filter);
            var doc = response.GetNextSet();
            foreach (var item in doc.ToArray())
            {
                Console.WriteLine(item);
                Console.WriteLine("lel");
            }
        }*/
    }
}
