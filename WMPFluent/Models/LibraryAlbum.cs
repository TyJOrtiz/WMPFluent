using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WMPFluent.Models
{
    public class LibraryAlbum : LibraryObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public LibraryAlbum()
        {
            Self = this;
        }
        [JsonIgnore]
        public LibraryAlbum Self { get; set; }
        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value; 
                NotifyPropertyChanged();
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }
        private string _artistName;
        public string ArtistName
        {
            get
            {
                return _artistName;
            }
            set
            {
                _artistName = value;
                NotifyPropertyChanged();
            }
        }
        private string _year;
        public string Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                NotifyPropertyChanged();
            }
        }
        private string _dateAdded;
        public string DateAdded
        {
            get 
            { 
                return _dateAdded; 
            }
            set 
            { 
                _dateAdded = value; 
                NotifyPropertyChanged(); 
            }
        }
        private string albumArt;
        public string AlbumArt
        {
            get { return albumArt; }
            set
            {
                albumArt = value; //GetAlbumDominateColor();
                NotifyPropertyChanged();
            }
        }
        private string genre;
        public string Genre
        {
            get { return genre; }
            set { genre = value; NotifyPropertyChanged(); }
        }
        [JsonIgnore]
        private bool isPlaying = false;
        [JsonIgnore]
        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                if (isPlaying != value)
                {
                    isPlaying = value; NotifyPropertyChanged();
                }
            }
        }
        private ObservableCollection<string> tagIds;
        public ObservableCollection<string> TagIds
        {
            get
            {
                return tagIds;
            }
            set
            {
                tagIds = value;
                NotifyPropertyChanged();
            }
        }
        private ObservableCollection<LibrarySong> songs;
        public ObservableCollection<LibrarySong> Songs
        {
            get { return songs; }
            set { songs = value; NotifyPropertyChanged(); }
        }
    }
}
