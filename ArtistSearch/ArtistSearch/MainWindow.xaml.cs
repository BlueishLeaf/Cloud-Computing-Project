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
    public partial class MainWindow
    {
        // Instantiate the control class
        private readonly Program _appInstance = new Program();

        public MainWindow()
        {
            InitializeComponent();

            // Populate Artist list box on load
            LbxAddArtists.ItemsSource = _appInstance.DbGetAllArtists();
        }

        //Region for the search tab of the application
        #region ArtistSearch
        private void BtnSearchArtist_Click(object sender, RoutedEventArgs e)
        {
            LbxMatchingArtists.ItemsSource = _appInstance.DbGetArtists(TbxEnterArtist.Text);
            TbkErrorOutput.Text = LbxMatchingArtists.Items.Count + " artists found";
            LbxMatchingAlbums.ItemsSource = null;
            LbxMatchingSongs.ItemsSource = null;
        }

        private void LbxMatchingArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(LbxMatchingArtists.SelectedItem is Artist selectedArtist)) return;
            selectedArtist.Albums = _appInstance.DbGetAlbums(selectedArtist);
            LbxMatchingAlbums.ItemsSource = selectedArtist.Albums;
            LbxMatchingSongs.ItemsSource = null;
        }

        private void LbxMatchingAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(LbxMatchingAlbums.SelectedItem is Album selectedAlbum)) return;
            selectedAlbum.Songs = _appInstance.DbGetSongs(selectedAlbum);
            LbxMatchingSongs.ItemsSource = selectedAlbum.Songs;
        }
        #endregion

        //Region for the add/edit tab of the application
        #region Add/Edit
        private void BtnAddArtist_Click(object sender, RoutedEventArgs e)
        {
            _appInstance.InsertArtist(TbxAddArtistName.Text);
            MessageBox.Show("Artist Created!");
            LbxAddArtists.ItemsSource = _appInstance.DbGetAllArtists();
            TbxAddArtistName.Text = "";

        }

        private void BtnAddAlbum_Click(object sender, RoutedEventArgs e)
        {
            if (!(LbxAddArtists.SelectedItem is Artist selectedArtist)) return;
            _appInstance.AppendAlbumToArtist(selectedArtist, TbxAddAlbumName.Text);
            MessageBox.Show("Album Created and Assigned!");
            LbxAddArtistAlbums.ItemsSource = _appInstance.DbGetAlbums(selectedArtist);
            TbxAddAlbumName.Text = "";
        }

        private void BtnAddSong_Click(object sender, RoutedEventArgs e)
        {
            if (!(LbxAddArtistAlbums.SelectedItem is Album selectedAlbum)) return;
            _appInstance.AppendSongToAlbum(selectedAlbum, TbxAddSongName.Text);
            MessageBox.Show("Song Created and Assigned!");
            LbxAddedSongs.ItemsSource = _appInstance.DbGetSongs(selectedAlbum);
            TbxAddSongName.Text = "";
        }

        private void LbxAddArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(LbxAddArtists.SelectedItem is Artist selectedArtist)) return;
            selectedArtist.Albums = _appInstance.DbGetAlbums(selectedArtist);
            LbxAddArtistAlbums.ItemsSource = selectedArtist.Albums;
            LbxAddedSongs.ItemsSource = null;
        }

        private void LbxAddArtistAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(LbxAddArtistAlbums.SelectedItem is Album selectedAlbum)) return;
            selectedAlbum.Songs = _appInstance.DbGetSongs(selectedAlbum);
            LbxAddedSongs.ItemsSource = selectedAlbum.Songs;
        }
        #endregion

        #region Delete
        private void BtnDeleteArtist_Click(object sender, RoutedEventArgs e)
        {
            var buttonContext = ((Button) sender).DataContext;
            var item = (ListBoxItem) LbxAddArtists.ItemContainerGenerator.ContainerFromItem(buttonContext);
            item.IsSelected = !item.IsSelected;
            if (!(LbxAddArtists.SelectedItem is Artist selectedArtist)) return;
            _appInstance.DeleteArtist(selectedArtist);
            MessageBox.Show("Artist Deleted!");
            LbxAddArtists.ItemsSource = _appInstance.DbGetAllArtists();
            LbxAddArtistAlbums.ItemsSource = null;
            LbxAddedSongs.ItemsSource = null;
        }

        private void BtnDeleteAlbum_Click(object sender, RoutedEventArgs e)
        {
            if (!(LbxAddArtists.SelectedItem is Artist selectedArtist)) return;
            var buttonContext = ((Button)sender).DataContext;
            var item = (ListBoxItem)LbxAddArtistAlbums.ItemContainerGenerator.ContainerFromItem(buttonContext);
            item.IsSelected = !item.IsSelected;
            if (!(LbxAddArtistAlbums.SelectedItem is Album selectedAlbum)) return;
            _appInstance.DeleteAlbum(selectedAlbum, selectedArtist);
            MessageBox.Show("Album Deleted!");
            LbxAddArtistAlbums.ItemsSource = _appInstance.DbGetAlbums(selectedArtist);
            LbxAddedSongs.ItemsSource = null;
        }

        private void BtnDeleteSong_Click(object sender, RoutedEventArgs e)
        {
            if (!(LbxAddArtistAlbums.SelectedItem is Album selectedAlbum)) return;
            var buttonContext = ((Button)sender).DataContext;
            var item = (ListBoxItem)LbxAddedSongs.ItemContainerGenerator.ContainerFromItem(buttonContext);
            item.IsSelected = !item.IsSelected;
            if (!(LbxAddedSongs.SelectedItem is Song selectedSong)) return;
            _appInstance.DeleteSong(selectedSong, selectedAlbum);
            MessageBox.Show("Song Deleted!");
            LbxAddedSongs.ItemsSource = _appInstance.DbGetSongs(selectedAlbum);
        }
        #endregion

        #region Edit
        private void BtnEditArtist_Click(object sender, RoutedEventArgs e)
        {
            //var buttonContext = ((Button)sender).DataContext;
            //var item = (ListBoxItem)LbxAddArtists.ItemContainerGenerator.ContainerFromItem(buttonContext);
            //item.IsSelected = !item.IsSelected;
            //if (!(LbxAddArtists.SelectedItem is Artist selectedArtist)) return;
            //_appInstance.EditArtist(this, selectedArtist);
            //LbxAddArtists.ItemsSource = _appInstance.DbGetAllArtists();
        }
        private void BtnEditAlbum_Click(object sender, RoutedEventArgs e)
        {
            //if (!(LbxAddArtists.SelectedItem is Artist selectedArtist)) return;
            //var buttonContext = ((Button)sender).DataContext;
            //var item = (ListBoxItem)LbxAddArtistAlbums.ItemContainerGenerator.ContainerFromItem(buttonContext);
            //item.IsSelected = !item.IsSelected;
            //if (!(LbxAddArtistAlbums.SelectedItem is Album selectedAlbum)) return;
            //_appInstance.EditAlbum(this, selectedAlbum);
            //LbxAddArtistAlbums.ItemsSource = _appInstance.DbGetAlbums(selectedArtist);
        }
        private void BtnEditSong_Click(object sender, RoutedEventArgs e)
        {
            //if (!(LbxAddArtistAlbums.SelectedItem is Album selectedAlbum)) return;
            //var buttonContext = ((Button)sender).DataContext;
            //var item = (ListBoxItem)LbxAddedSongs.ItemContainerGenerator.ContainerFromItem(buttonContext);
            //item.IsSelected = !item.IsSelected;
            //if (!(LbxAddedSongs.SelectedItem is Song selectedSong)) return;
            //_appInstance.EditSong(this,selectedSong);
            //LbxAddedSongs.ItemsSource = _appInstance.DbGetSongs(selectedAlbum);
        }
        #endregion

        #region Backup
        private void BtnBackupS3_Click(object sender, RoutedEventArgs e)
        {
            _appInstance.BackupToS3();
        }
        #endregion
    }
}
