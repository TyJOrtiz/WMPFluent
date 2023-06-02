using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace WMPFluent.Models
{
    public class LibrarySong : LibraryObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LibrarySong()
        {
            Self = this;
        }

        [JsonIgnore]
        public LibrarySong Self { get; set; }
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
        private string album;
        public string Album
        {
            get { return album; }
            set { album = value; NotifyPropertyChanged(); }
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
        private string _songArtistName;
        public string SongArtistName
        {
            get
            {
                return _songArtistName;
            }
            set
            {
                _songArtistName = value;
                NotifyPropertyChanged();
            }
        }
        private string albumArtist;
        public string AlbumArtist
        {
            get { return albumArtist; }
            set { albumArtist = value; NotifyPropertyChanged(); }
        }
        private string releaseYear;
        public string ReleaseYear
        {
            get { return releaseYear; }
            set { releaseYear = value; NotifyPropertyChanged(); }
        }
        private string dateAdded;
        public string DateAdded
        {
            get { return dateAdded; }
            set { dateAdded = value; NotifyPropertyChanged(); }
        }
        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; NotifyPropertyChanged(); }
        }
        private int track;
        public int Track
        {
            get { return track; }
            set { track = value; NotifyPropertyChanged(); }
        }
        private bool isExplicit = false;
        public bool IsExplicit
        {
            get { return isExplicit; }
            set { isExplicit = value; NotifyPropertyChanged(); }
        }
        private bool liked = false;
        public bool Liked
        {
            get { return liked; }
            set { liked = value; NotifyPropertyChanged(); }
        }
        private string duration;
        public string Duration
        {
            get { return duration; }
            set { duration = value; NotifyPropertyChanged(); }
        }
        [JsonIgnore]
        public LibraryAlbum ResidingAlbum { get; set; }
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
        private int playCount;
        public int PlayCount
        {
            get { return playCount; }
            set { playCount = value; NotifyPropertyChanged(); }
        }
        private string lastPlayedTime;
        public string LastPlayedTime
        {
            get { return lastPlayedTime; }
            set { lastPlayedTime = value; NotifyPropertyChanged(); }
        }
        private string startTime;
        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; NotifyPropertyChanged(); }
        }
        private string endTime;
        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; NotifyPropertyChanged(); }
        }
        private string genre;
        public string Genre
        {
            get { return genre; }
            set { genre = value; NotifyPropertyChanged(); }
        }
        private string albumIdentifier;
        public string AlbumIdentifier
        {
            get { return albumIdentifier; }
            set { albumIdentifier = value; NotifyPropertyChanged(); }
        }
        private string albumArt;
        public string AlbumArt
        {
            get { return albumArt; }
            set { albumArt = value; NotifyPropertyChanged(); }
        }
    }
}