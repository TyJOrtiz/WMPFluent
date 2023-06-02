using WMPFluent.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WMPFluent.ViewModels
{
    internal class TagViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Tag tag;
        public Tag Tag
        {
            get { return tag; }
            set
            {
                if (tag == value) return;
                tag = value;
                NotifyPropertyChanged();
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<LibrarySong> songs;
        public ObservableCollection<LibrarySong> Songs
        {
            get { return songs; }
            set { songs = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<LibraryAlbum> albums;
        public ObservableCollection<LibraryAlbum> Albums 
        { 
            get { return albums; }
            set { albums = value; NotifyPropertyChanged(); }
        }

        public TagViewModel(Tag value, string name)
        {
            this.Tag = value;
            this.Name = name;
            this.Tag.ItemUpdatedToTag += Tag_ItemAddedToTag;
            Albums = new ObservableCollection<LibraryAlbum>(App.AppViewModel.LibraryAlbums.Where(x => x.TagIds != null && x.TagIds.Contains(Tag.Id)));
        }
        public void RemoveNotification()
        {
            this.Tag.ItemUpdatedToTag -= Tag_ItemAddedToTag;
        }
        private void Tag_ItemAddedToTag(object sender, System.EventArgs e)
        {
            switch ((e as TagUpdatedEventArgs).Reason)
            {
                case TagUpdateReason.ItemAdded:
                    if ((e as TagUpdatedEventArgs).ObjectAdded is LibraryAlbum la)
                    {
                        this.albums.Add(la);
                    }
                    break; 
                case TagUpdateReason.ItemRemoved:
                    if ((e as TagUpdatedEventArgs).ObjectRemoved is LibraryAlbum a)
                    {
                        try
                        {
                            this.albums.Remove(a);
                        }
                        catch
                        {

                        }
                    }
                    break;
                default:
                    break;

            }
        }
    }
}