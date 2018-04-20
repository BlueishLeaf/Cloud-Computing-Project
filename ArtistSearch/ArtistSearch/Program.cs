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

        public string DbGetArtists(string artist)
        {
            return Table.LoadTable(Client, "Artists").GetItem(artist)["Artist"];
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
