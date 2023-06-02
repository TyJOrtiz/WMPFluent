using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playlists;
using Windows.Storage;
using Windows.UI.Xaml.Shapes;

namespace WMPFluent.ViewModels
{
    public class SelectedPlaylistPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LibrarySong> _songs;
        public ObservableCollection<LibrarySong> Songs
        {
            get { return _songs; }
            set
            {
                _songs = value;
                NotifyPropertyChanged();
            }
        }

        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; NotifyPropertyChanged("Path"); }
        }
        private string _playlistName;
        public string PlaylistName
        {
            get { return _playlistName; }
            set
            {
                if (_playlistName != value)
                {
                    _playlistName = value;
                    NotifyPropertyChanged("PlaylistName");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SelectedPlaylistPageViewModel(string path, string name)
        {
            this.PlaylistName = name;
            this.Path = path;
            Songs = new ObservableCollection<LibrarySong>();
            LoadSongs();
        }

        public async void LoadSongs()
        {
            Songs.Clear();
            var file = await StorageFile.GetFileFromPathAsync(Path);
            if (System.IO.Path.GetExtension(file.Path).Equals(".wpl", StringComparison.InvariantCultureIgnoreCase) || System.IO.Path.GetExtension(file.Path).Equals(".zpl", StringComparison.InvariantCultureIgnoreCase))
            {
                var playlistfile = await Windows.Media.Playlists.Playlist.LoadAsync(file);
                var lines = await FileIO.ReadLinesAsync(file);
                lines = playlistfile.Files.Select(x => x.Path).ToList();
                foreach (var item in lines)
                {
                    if (App.AppViewModel.LibraryAlbums.SelectMany(x => x.Songs).Where(s => s.Path == item).Any())
                    {
                        Songs.Add(App.AppViewModel.LibraryAlbums.SelectMany(x => x.Songs).Where(s => s.Path == item).FirstOrDefault());
                    }
                }

            }
            else if (System.IO.Path.GetExtension(file.Path).Contains(".m3u", StringComparison.InvariantCultureIgnoreCase))
            {
                var lines = await FileIO.ReadLinesAsync(file);
                lines = lines.Where(x => !x.StartsWith("#EXT")).ToList();
                foreach (var item in lines)
                {
                    if (App.AppViewModel.LibraryAlbums.SelectMany(x => x.Songs).Where(s => s.Path == item).Any())
                    {
                        Songs.Add(App.AppViewModel.LibraryAlbums.SelectMany(x => x.Songs).Where(s => s.Path == item).FirstOrDefault());
                    }
                }
            }
            CalculateTotalRunTime();
        }
        public string[] formats = new string[]
        {
            "mm\\:ss",
            "hh\\:mm\\:ss"
        };
        private void CalculateTotalRunTime()
        {
            TimeSpan timeSpan;
            foreach (var item in Songs)
            {
                ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture));
                ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture).Minutes);
                ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture).Seconds);
                timeSpan += (TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture));
            }
            ////Debug.WriteLine(timeSpan);
            string timeFormat = Songs.Count > 1 ? $"{Songs.Count} songs · " : $"1 song · ";
            if (timeSpan.Hours > 0)
            {
                timeFormat += timeSpan.Hours > 1 ? $"{timeSpan.Hours} hours " : $"{timeSpan.Hours} hour ";
            }
            if (timeSpan.Minutes > 0)
            {
                timeFormat += timeSpan.Minutes > 1 ? $"{timeSpan.Minutes} minutes" : $"{timeSpan.Minutes} minutes";
            }
            PlaylistInfo = timeFormat;

        }
        private string playlistInfo;
        public string PlaylistInfo
        {
            get
            {
                return playlistInfo;
            }
            set
            {
                playlistInfo = value;
                NotifyPropertyChanged("PlaylistInfo");
            }
        }
    }
}
