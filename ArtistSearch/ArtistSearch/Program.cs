using System;
using System.Collections;
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
        private static readonly AmazonDynamoDBClient Client =
            new AmazonDynamoDBClient(new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.EUWest1});

        public List<Artist> DbGetArtists(string a)
        {
            try
            {
                //Artist name to lower
                var artist = a.ToLower();

                //Load artists table
                var table = Table.LoadTable(Client, "Artist");

                //Filter for scan. Artist column matches artist input
                var filter = new ScanFilter();
                filter.AddCondition("ArtistName", ScanOperator.Contains, artist);

                //Response
                var response = table.Scan(filter).GetNextSet();

                //List to contain found artists
                var foundArtists = new List<Artist>();

                //Loop through each item in the response and map it to a new Artist in the artist list
                foreach (var item in response)
                {
                    foundArtists.Add(new Artist()
                    {
                        ArtistName = item["ArtistName"],
                        ArtistId = int.Parse(item["ArtistID"])
                    });
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
                //Load albums table
                var table = Table.LoadTable(Client, "Album");

                //Filter for query. Artist column matches artist input
                var filter = new QueryFilter();
                filter.AddCondition("ArtistName", QueryOperator.Equal, artist.ArtistName);

                //Response
                var response = table.Query(filter).GetNextSet();

                //List to contain found artists
                var foundAlbums = new List<Album>();

                //Loop through each item in the response and map it to a new Artist in the artist list
                foreach (var item in response)
                {
                    foundAlbums.Add(new Album() {AlbumName = item["AlbumName"]});
                }

                return foundAlbums;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void InsertArtist(string artistName)
        {
            //Random number generation for the ID
            var rndm = new Random();
            //Loads the artist table and inserts a new item
            Table.LoadTable(Client, "Artist").PutItem(new Document
            {
                ["ArtistName"] = artistName.ToLower(),
                ["ArtistID"] = rndm.Next(100, 100000)
            });
        }

        public void AppendAlbumToArtist(Artist artist, string albumName)
        {
            //Loads the album table and inserts a new item
            Table.LoadTable(Client, "Album").PutItem(new Document
            {
                ["ArtistName"] = artist.ArtistName,
                ["AlbumName"] = albumName.ToLower()
            });
        }

        public void AppendSongToAlbum(Album album, string songName)
        {
            //Loads the song table and inserts a new item
            Table.LoadTable(Client, "Song").PutItem(new Document
            {
                ["AlbumName"] = album.AlbumName,
                ["SongName"] = songName.ToLower()
            });
        }

        public void DeleteArtist(Artist artist)
        {
            Client.DeleteItem(new DeleteItemRequest
            {
                TableName = "Artist",
                Key = new Dictionary<string, AttributeValue>()
                {
                    {"ArtistID", new AttributeValue {N = artist.ArtistId.ToString()}},
                    {"ArtistName", new AttributeValue {S = artist.ArtistName}}
                }
            });
        }

        public void DeleteAlbum(Album album, Artist artist)
        {
            Client.DeleteItem(new DeleteItemRequest
            {
                TableName = "Album",
                Key = new Dictionary<string, AttributeValue>()
                {
                    {"ArtistName", new AttributeValue {S = artist.ArtistName}},
                    {"AlbumName", new AttributeValue {S = album.AlbumName}}
                }
            });
        }

        public void DeleteSong(Song song, Album album)
        {
            Client.DeleteItem(new DeleteItemRequest
            {
                TableName = "Song",
                Key = new Dictionary<string, AttributeValue>()
                {
                    {"AlbumName", new AttributeValue {S = album.AlbumName}},
                    {"SongName", new AttributeValue {S = song.SongName}}
                }
            });
        }

        public List<Artist> DbGetAllArtists()
        {
            try
            {
                //Load artist table
                var table = Table.LoadTable(Client, "Artist");

                //Scan filter and response
                var filter = new ScanFilter();
                filter.AddCondition("ArtistName", ScanOperator.IsNotNull);
                var response = table.Scan(filter).GetNextSet();

                //List to contain found artists
                var foundArtists = new List<Artist>();

                //Loop through each item in the response and map it to a new Artist in the artist list
                foreach (var item in response)
                {
                    foundArtists.Add(new Artist()
                    {
                        ArtistName = item["ArtistName"],
                        ArtistId = int.Parse(item["ArtistID"])
                    });
                }

                return foundArtists;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public List<Song> DbGetSongs(Album album)
        {
            try
            {
                //Load songs table
                var table = Table.LoadTable(Client, "Song");

                //Filter for query. AlbumName column matches album input
                var filter = new QueryFilter();
                filter.AddCondition("AlbumName", QueryOperator.Equal, album.AlbumName);

                //Response
                var response = table.Query(filter).GetNextSet();

                //List to contain found songs
                var foundSongs = new List<Song>();

                //Loop through each item in the response and map it to a new Song in the song list
                foreach (var item in response)
                {
                    foundSongs.Add(new Song() {SongName = item["SongName"]});
                }

                return foundSongs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //public void EditArtist(MainWindow main, Artist artist)
        //{
        //    var edit = new Edit() { Owner = main };
        //    edit.ShowDialog();
        //    Console.WriteLine(edit.TbxNewValue.Text);
        //    Client.UpdateItem(new UpdateItemRequest()
        //    {
        //        TableName = "Artist",
        //        Key = new Dictionary<string, AttributeValue>() { { "ArtistID", new AttributeValue { N = artist.ArtistId.ToString() } }, { "ArtistName", new AttributeValue { S = artist.ArtistName } } },
        //        ExpressionAttributeNames = new Dictionary<string, string>() { { "#A","ArtistName"} },
        //        ExpressionAttributeValues = new Dictionary<string, AttributeValue>() { { ":a", new AttributeValue() { S = edit.TbxNewValue.Text} } },
        //        UpdateExpression = "SET #A = :a"
        //    });
        //}
    }
}
