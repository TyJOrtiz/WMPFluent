using WMPFluent.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WMPFluent.Models
{
    public class LibraryArtist : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string artist;

        public LibraryArtist(string artist, System.Collections.ObjectModel.ObservableCollection<LibraryAlbum> libraryAlbums)
        {
            this.Artist = artist;
            this.ArtistAlbums = libraryAlbums; //only for play/shuffle commands
            //GetArtistImage();
        }
        private ImageSource artistImage;
        public ImageSource ArtistImage
        {
            get
            {
                return artistImage;
            }
            set
            {
                artistImage = value;
                NotifyPropertyChanged();
            }
        }
        public async void GetArtistImage()
        {
            try
            {
                var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("ArtistBackgroundImages");
                var img = await folder.GetFileAsync($"{Artist.MakeSafeForFileName()}.jpg");
                ArtistImage = new BitmapImage(new Uri(img.Path));
                HasArtistImage = true;
                IsBlank = false;
            }
            catch
            {

            }
        }

        private bool isBlank = true;
        public bool IsBlank
        {
            get
            {
                return isBlank;
            }
            set
            {
                if (isBlank != value)
                {
                    isBlank = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private bool hasArtistImage = false;
        public bool HasArtistImage
        {
            get
            {
                return hasArtistImage;
            }
            set
            {
                if (hasArtistImage != value)
                {
                    hasArtistImage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Artist
        {
            get
            {
                return artist;
            }
            set
            {
                artist = value;
                NotifyPropertyChanged();

            }
        }

        public ObservableCollection<LibraryAlbum> ArtistAlbums { get; private set; }
    }
}