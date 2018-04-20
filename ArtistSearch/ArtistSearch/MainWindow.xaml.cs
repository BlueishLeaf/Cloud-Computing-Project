using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArtistSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Artist> artistsList = new List<Artist>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSearchArtist_Click(object sender, RoutedEventArgs e)
        {
            //If theres an arist entered
            if (TbxEnterArtist.Text.Length > 0)
            {
                //Run Query to search DynamoDB artist table for matching artists
                artistsList = GetMatchingArtists(TbxEnterArtist.Text);

                //Output number of matching artists found to TbkErrorOutput
                TbkErrorOutput.Foreground = new SolidColorBrush(Colors.Green);
                TbkErrorOutput.Text = (artistsList.Count + " matching artists found");

                //Output all matching artists to LbxMatchingArtists
            }
            else
            {
                //Output error because no search term
                TbkErrorOutput.Foreground = new SolidColorBrush(Colors.Red);
                TbkErrorOutput.Text = "Please enter an aritst name";
            }
        }
        private List<Artist> GetMatchingArtists(string searchTerm)
        {
            List<Artist> matchingArtists = new List<Artist>();

            return matchingArtists;
        }

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
}
