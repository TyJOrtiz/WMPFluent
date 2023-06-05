using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Media.Playback;

namespace WMPFluent.Models
{
    public class NowPlayingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NowPlayingItem()
        {
            Self = this;
            ThemeListener themeListener = new ThemeListener();
            themeListener.ThemeChanged += ThemeListener_ThemeChanged;
        }

        private void ThemeListener_ThemeChanged(ThemeListener sender)
        {
            this.NotifyPropertyChanged("IsNowPlaying");
        }

        public MediaPlaybackItem MediaPlaybackItem { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string Time { get; set; }
        public string AlbumIdentifier { get; internal set; }
        private bool isNowPlaying = false;
        public bool IsNowPlaying
        {
            get { return isNowPlaying; }
            set
            {
                if (isNowPlaying != value)
                {
                    isNowPlaying = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public NowPlayingItem Self { get; private set; }
    }
}