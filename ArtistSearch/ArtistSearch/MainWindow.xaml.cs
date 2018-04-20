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

        private void BtnSearchArtist_Click(object sender, RoutedEventArgs e)
        {
            LbxMatchingArtists.Items.Add(_appInstance.DbGetArtists(TbxEnterArtist.Text));
        }

        private void LbxMatchingArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _appInstance.DbGetAlbums(LbxMatchingArtists.SelectedItem as string);
        }

        private void LbxMatchingAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
