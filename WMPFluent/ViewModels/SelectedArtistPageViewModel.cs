using Microsoft.Toolkit.Collections;
using WMPFluent.Extensions;
using WMPFluent.Models;
//using WMPFluent.NavigationPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.StartScreen;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace WMPFluent.ViewModels
{
    public class SelectedArtistPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ObservableCollection<LibraryAlbum> _albums;
        public ObservableCollection<LibraryAlbum> Albums
        {
            get { return _albums; }
            set
            {
                if (_albums != value)
                {
                    _albums = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private Thickness headerThickness;
        public Thickness HeaderThickness
        {
            get { return headerThickness; }
            set
            {
                headerThickness = value;
                NotifyPropertyChanged();
            }
        }
        private string artist;
        public string Artist
        {
            get { return artist; }
            set
            {
                if (artist != value)
                {
                    artist = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ImageSource artistImagePath = null;
        public ImageSource ArtistImagePath
        {
            get { return artistImagePath; }
            set
            {
                artistImagePath = value;
                NotifyPropertyChanged();
            }
        }
        public SelectedArtistPageViewModel(string artist)
        {
            Artist = artist;
            HeaderThickness = new Thickness(0);
            GetListOfFeaturedAlbums();
            TryLoadImage();
            LoadCollection();
            
        }
        private ObservableCollection<LibrarySong> songs;
        public ObservableCollection<LibrarySong> Songs
        {
            get { return songs; }
            set
            {
                songs = value;
                NotifyPropertyChanged();
            }
        }
        public void LoadCollection()
        {
            Albums = new ObservableCollection<LibraryAlbum>(App.AppViewModel.LibraryAlbums.Where(x => x.ArtistName == Artist).OrderByDescending(x => x.Year));
            Songs = new ObservableCollection<LibrarySong>(Albums.SelectMany(x => x.Songs));
            foreach (var album in Albums)
            {
                foreach (var song in album.Songs.ToList())
                {
                    song.ResidingAlbum = album;
                }
            }
            var songgroups = from c in Albums.SelectMany(x => x.Songs).OrderBy(x => (Convert.ToInt32(x.Track)))
                             group c by c.ResidingAlbum;
            this.SongSource = new ObservableGroupedCollection<object, object>(songgroups.OrderByDescending(x => x.Key.Year));
            this.CollectionView = new CollectionViewSource
            {
                IsSourceGrouped = true,
                Source = this.SongSource,
            };
        }
        private CollectionViewSource collectionView;
        public CollectionViewSource CollectionView
        {
            get { return collectionView; }
            set
            {
                collectionView = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableGroupedCollection<object, object> songSource;

        public ObservableGroupedCollection<object, object> SongSource
        {
            get { return songSource; }
            set
            {
                songSource = value;
                NotifyPropertyChanged();
            }
        }
        private ObservableCollection<LibrarySong> featuredsongList;
        public ObservableCollection<LibrarySong> FeaturedSongList
        {
            get { return featuredsongList; }
            set
            {
                featuredsongList = value;
                NotifyPropertyChanged();
            }
        }
        private void GetListOfFeaturedAlbums()
        {
            if (Artist == "Various Artists" || Artist == "Various artists")
            {
                return;
            }
            FeaturedSongList = new ObservableCollection<LibrarySong>(App.AppViewModel.LibrarySongs.Where(s => (s.SongArtistName.Contains(Artist, StringComparison.OrdinalIgnoreCase) || s.HasFeature(Artist)) && s.AlbumArtist != Artist));
            if (FeaturedSongList.Any())
            {
                ShowFeatures = true;
            }
        }
        private bool showFeatures = false;
        public bool ShowFeatures
        {
            get { return showFeatures; }
            set
            {
                if (showFeatures != value)
                {
                    showFeatures = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private bool artistImageLoaded = false;
        public bool ArtistImageLoaded
        {
            get { return artistImageLoaded; }
            set
            {
                if (artistImageLoaded != value)
                {
                    artistImageLoaded = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private bool noArtistImage = true;
        private bool albumSelected = false;

        public bool NoArtistImage
        {
            get { return noArtistImage; }
            set
            {
                if (noArtistImage != value)
                {
                    noArtistImage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool AlbumSelected {
            get
            {
                return albumSelected;
            }
            set
            {
                if (albumSelected != value)
                {
                    albumSelected = value;
                    NotifyPropertyChanged();
                }
            } 
        }

        private ObservableCollection<LibrarySong> selectedTrackItems;
        public ObservableCollection<LibrarySong> SelectedTrackItems
        {
            get
            {
                return selectedTrackItems;
            }
            set
            {
                selectedTrackItems = value;
                NotifyPropertyChanged();
            }
        }
        private bool albumListMutlipleSelected = false;
        public bool AlbumListMultipleSelected
        {
            get
            {
                return albumListMutlipleSelected;
            }
            set
            {
                if (albumListMutlipleSelected != value)
                {
                    albumListMutlipleSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private async void TryLoadImage()
        {
            try
            {
                var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("ArtistBackgroundImages");
                var img = await folder.GetFileAsync($"{Artist.MakeSafeForFileName()}.jpg");
                if (img != null)
                {
                    ArtistImagePath = new BitmapImage(new Uri(img.Path));
                    ArtistImageLoaded = true;
                    NoArtistImage = false;
                    HeaderThickness = new Thickness(20);
                }
            }
            catch
            {
                var artistImage = await Helpers.WebHelpers.GetImageForArtist(Artist);
                if (!string.IsNullOrEmpty(artistImage))
                {
                    ArtistImagePath = new BitmapImage(new Uri(artistImage));
                    ArtistImageLoaded = true;
                    NoArtistImage = false;
                    HeaderThickness = new Thickness(20);
                    try
                    {
                        var client = new WebClient();
                        byte[] imageBytes = client.DownloadData(artistImage);
                        var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ArtistBackgroundImages", CreationCollisionOption.OpenIfExists);
                        var img = await folder.CreateFileAsync($"{Artist.MakeSafeForFileName()}.jpg", CreationCollisionOption.ReplaceExisting);
                        // Save the image to a file
                        File.WriteAllBytes(img.Path, imageBytes);
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
