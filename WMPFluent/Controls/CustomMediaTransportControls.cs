using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.Networking.NetworkOperators;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WMPFluent.Interactions;

namespace WMPFluent.Controls
{
    public class CustomMediaTransportControls : MediaTransportControls
    {
        public CustomMediaTransportControls()
        {
            this.DefaultStyleKey = typeof(CustomMediaTransportControls);
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }

        

        public MediaPlaybackItem CurrentPlaybackItem
        {
            get { return _mediaPlaybackItem; }
            set
            {
                if (_mediaPlaybackItem != value)
                {
                    _mediaPlaybackItem = value;
                    OnMediaPlaybackItemChanged(value);
                }
            }
        }

        private void OnMediaPlaybackItemChanged(MediaPlaybackItem value)
        {
            if (value != null)
            {
                try
                {
                    this.TrackName = (string)value.Source.CustomProperties["Title"];
                    this.ArtistName = (string)value.Source.CustomProperties["Artist"];
                    this.TrackThumbnail = new BitmapImage(new Uri((string)value.Source.CustomProperties["AlbumArt"]));
                }
                catch
                {

                }
            }
            else
            {
                this.TrackName = null;
                this.ArtistName = null;
                this.TrackThumbnail = null;
            }
        }

        public static readonly DependencyProperty TrackNameProperty =
       DependencyProperty.Register("TrackName", typeof(string), typeof(CustomMediaTransportControls), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTrackNamePropertyChanged)));

        private static void OnTrackNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomMediaTransportControls control = (CustomMediaTransportControls)d;
            if (e.NewValue != null)
            {
                control.TrackName = e.NewValue as string;
            }
            else
            {
                control.TrackName = null;
            }
        }

        public static readonly DependencyProperty ArtistNameProperty =
            DependencyProperty.Register("ArtistName", typeof(string), typeof(CustomMediaTransportControls), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnArtistNamePropertyChanged)));

        private static void OnArtistNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomMediaTransportControls control = (CustomMediaTransportControls)d;
            if (e.NewValue != null)
            {
                control.ArtistName = e.NewValue as string;
            }
            else
            {
                control.ArtistName = null;
            }
        }

        public string TrackName
        {
            get { return (string)GetValue(TrackNameProperty); }
            set { SetValue(TrackNameProperty, value); }
        }

        public string ArtistName
        {
            get { return (string)GetValue(ArtistNameProperty); }
            set { SetValue(ArtistNameProperty, value); }
        }

        public ImageSource TrackThumbnail
        {
            get
            {
                return (ImageSource)GetValue(TrackThumbnailProperty);
            }
            set
            {
                SetValue(TrackThumbnailProperty, value);
            }
        }
        public static readonly DependencyProperty TrackThumbnailProperty = DependencyProperty.Register("TrackThumbnail",
            typeof(ImageSource),
            typeof(CustomMediaTransportControls),
            new PropertyMetadata(null, new PropertyChangedCallback(OnTrackThumbnailChanged)));
        private MediaPlaybackItem _mediaPlaybackItem;

        private static void OnTrackThumbnailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomMediaTransportControls control = (CustomMediaTransportControls)d;
            if (e.NewValue != null)
            {
                if (e.NewValue.GetType() == typeof(string))
                {
                    try
                    {
                        control.TrackThumbnail = new BitmapImage(new Uri((string)e.NewValue));
                    }
                    catch
                    {
                        control.TrackThumbnail = null;
                    }
                }
                else if (e.NewValue.GetType() == typeof(BitmapImage))
                {
                    control.TrackThumbnail = (BitmapImage)e.NewValue;
                }
                else if (e.NewValue.GetType() == typeof(Uri))
                {
                    try
                    {
                        control.TrackThumbnail = new BitmapImage((Uri)e.NewValue);
                    }
                    catch
                    {
                        control.TrackThumbnail = null;
                    }
                }
            }
            else
            {
                control.TrackThumbnail = null;
            }
        }
    }
}
