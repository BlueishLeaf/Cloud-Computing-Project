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
        private readonly Program _appInstance = new Program();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void BtnSearchArtist_Click(object sender, RoutedEventArgs e)
        {
            LbxMatchingArtists.ItemsSource = "";
            LbxMatchingArtists.ItemsSource = _appInstance.DbGetArtists(TbxEnterArtist.Text);
            TbkErrorOutput.Text = LbxMatchingArtists.Items.Count + " artists found";
        }

        private void LbxMatchingArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Artist selectedArtist = LbxMatchingArtists.SelectedItem as Artist;

            selectedArtist.Albums = _appInstance.DbGetAlbums(selectedArtist);

            LbxMatchingAlbums.ItemsSource = selectedArtist.Albums;
        }

        private void LbxMatchingAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
