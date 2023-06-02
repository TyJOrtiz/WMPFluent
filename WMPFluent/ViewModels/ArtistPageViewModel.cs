using Microsoft.Toolkit.Collections;
using Newtonsoft.Json.Linq;
using WMPFluent.Extensions;
using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WMPFluent.ViewModels
{
    public class ArtistPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void FilterQuery(string text)
        {
            if (text.Length > 0)
            {
                var d = from a in Artists.Where(x => x.Artist.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x.Artist) group a by a.Artist.ToUpper().First().CheckChar().DeAccent();
                this.ArtistSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = true,
                    Source = this.ArtistSource,
                };
            }
            else
            {
                var d = from a in Artists.OrderBy(x => x.Artist) group a by a.Artist.ToUpper().First().CheckChar().DeAccent();
                this.ArtistSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = true,
                    Source = this.ArtistSource,
                };
            }
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

        private ObservableGroupedCollection<string, object> artistSource;

        public ObservableGroupedCollection<string, object> ArtistSource
        {
            get { return artistSource; }
            set
            {
                artistSource = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<LibraryArtist> artists;
        public ObservableCollection<LibraryArtist> Artists
        {
            get
            {
                return artists;
            }
            set
            {
                artists = value;
                NotifyPropertyChanged();
            }
        }
        public ArtistPageViewModel()
        {
            Artists = new ObservableCollection<Models.LibraryArtist>(App.AppViewModel.LibraryAlbums.Select(x => x.ArtistName).Distinct().Select(a => new Models.LibraryArtist(a, new ObservableCollection<LibraryAlbum>(App.AppViewModel.LibraryAlbums.Where(x => x.ArtistName == a)))));
            var d = from a in Artists.OrderBy(x => x.Artist) group a by a.Artist.ToUpper().First().CheckChar().DeAccent();
            this.ArtistSource = new ObservableGroupedCollection<string, object>(d);
            this.CollectionView = new CollectionViewSource
            {
                IsSourceGrouped = true,
                Source = this.ArtistSource,
            };
        }
    }
}
