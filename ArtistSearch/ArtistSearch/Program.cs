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

        public List<string> DbGetArtists(string a)
        {
            //return Table.LoadTable(Client, "Artists").GetItem(artist)["Artist"];

            try
            {
                //Artist name to lower
                string artist = a.ToLower();
                Console.WriteLine("Getting matching artists containing " + a);

                //Load artists table
                var table = Table.LoadTable(Client, "Artists");

                //Filter for query. Artist column matches artist input
                ScanFilter filter = new ScanFilter();
                filter.AddCondition("ArtistName", ScanOperator.Contains, artist);
                Console.WriteLine("Filter created");

                //Response
                var response = table.Scan(filter).GetNextSet();
                Console.WriteLine("scan completed");

                //List to hold output of found artists
                List<string> foundArtists = new List<string>();

                foreach (var item in response.ToArray())
                {
                    var currentItem = ((item.ToArray())[0].ToString().Split(','))[1].TrimEnd(']');
                    foundArtists.Add(currentItem.ToString());
                    Console.WriteLine(currentItem.ToString());
                }

                return foundArtists;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void DbGetAlbums(string artist)
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
        }
    }
}
