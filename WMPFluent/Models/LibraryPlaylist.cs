using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace WMPFluent.Models
{
    public class LibraryPlaylist : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string path;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                NotifyPropertyChanged();
            }
        }
        public async void Rename(string name)
        {
            var file = await StorageFile.GetFileFromPathAsync(Path);
            await file.RenameAsync(name, NameCollisionOption.GenerateUniqueName);
            this.Name = file.DisplayName;
            this.Path = file.Path;
            this.PlaylistObject.Path = file.Path;
            //this.PlaylistObject.Label = file.DisplayName;
        }
        public LibraryPlaylist(string path)
        {
            this.Path = path;
        }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }
        public PlaylistObject PlaylistObject { get; set; }
    }
}