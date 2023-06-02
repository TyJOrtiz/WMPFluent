using WMPFluent.Extensions;
using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WMPFluent.ViewModels
{
    public class SelectedAlbumViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SelectedAlbumViewModel(LibraryAlbum libraryAlbum) 
        {
            this.Album = libraryAlbum;
            CalculateTotalRunTime(Album);
        }
        public string[] formats = new string[]
        {
            "mm\\:ss",
            "hh\\:mm\\:ss"
        };
        private void CalculateTotalRunTime(LibraryAlbum album)
        {
            TimeSpan timeSpan;
            foreach(var item in album.Songs)
            {
                ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture));
                ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture).Minutes);
                ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture).Seconds);
                timeSpan += (TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture));
            }
            ////Debug.WriteLine(timeSpan);
            string timeFormat = album.Songs.Count > 1 ? String.Format("MultipleSongs".GetLocalizedString(), album.Songs.Count) + " · " : String.Format("SingleSong".GetLocalizedString(), album.Songs.Count) + " · ";
            if (timeSpan.Hours > 0)
            {
                timeFormat += timeSpan.Hours > 1 ? String.Format("MultipleHours".GetLocalizedString(), timeSpan.Hours) + " " : String.Format("SingleHour".GetLocalizedString(), timeSpan.Hours) + " ";
                //timeFormat += timeSpan.Hours > 1 ? $"{timeSpan.Hours} hours " : $"{timeSpan.Hours} hour ";
            }
            if (timeSpan.Minutes > 0)
            {
                timeFormat += timeSpan.Minutes > 1 ? String.Format("MultipleMinutes".GetLocalizedString(), timeSpan.Minutes) : String.Format("SingleMinute".GetLocalizedString(), timeSpan.Minutes);
            }
            AlbumInfo = timeFormat;

        }
        private string albumInfo;
        public string AlbumInfo
        {
            get
            {
                return albumInfo;
            }
            set
            {
                albumInfo = value;
                NotifyPropertyChanged();
            }
        }
        public LibraryAlbum Album { get; set; }
    }
}
