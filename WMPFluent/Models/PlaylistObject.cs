using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace WMPFluent.Models
{
    public class PlaylistObject : NavigationObject, INotifyPropertyChanged
    {
        public PlaylistObject(string path)
        {
            this.Path = path;
            GetPlaylistImage();
        }

        private void GetPlaylistImage()
        {
            //throw new NotImplementedException();
        }

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
        private ImageSource playlistImage;
        public ImageSource PlaylistImage
        {
            get
            {
                return playlistImage;
            }
            set
            {
                playlistImage = value;
                NotifyPropertyChanged();
            }
        }
    }
}
