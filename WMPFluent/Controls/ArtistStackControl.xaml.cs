using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WMPFluent.Extensions;
using WMPFluent.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WMPFluent.Controls
{
    public sealed partial class ArtistStackControl : UserControl
    {
        public LibraryArtist artist { get { return this.DataContext as LibraryArtist; } }
        public ArtistStackControl()
        {
            this.InitializeComponent();
            //BuildStack(artist);
            this.DataContextChanged += (d, c) =>
            {
                this.Bindings.Update();
            };
        }

        public void BuildStack(LibraryArtist artist)
        { 
            if (StackHost.Tag == artist)
            {
                StackHost.Children.Clear();
                int degree = 0;
                double opacity = 1;
                foreach (var item in artist.ArtistAlbums.Take(6))
                {
                    var image = new ImageEx
                    {
                        Source = item.AlbumArt,
                        Height = 63.64,
                        Width = 63.64,
                        Opacity = opacity,
                        CenterPoint = new System.Numerics.Vector3((float)31.82, (float)31.82, 0),
                        CornerRadius = new CornerRadius(2),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Rotation = degree,
                        RotationAxis = System.Numerics.Vector3.UnitZ
                    };
                    StackHost.Children.Insert(0, image);
                    degree = degree - 15;
                    opacity = opacity - 0.1;
                }
            }
            CalculateTotalRunTime(artist.ArtistAlbums);
        }

        public string[] formats = new string[]
        {
            "mm\\:ss",
            "hh\\:mm\\:ss"
        };
        private void CalculateTotalRunTime(ObservableCollection<LibraryAlbum> artistAlbums)
        {
            TimeSpan timeSpan;
            foreach (var album in artistAlbums)
            {
                foreach (var item in album.Songs)
                {
                    ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture));
                    ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture).Minutes);
                    ////Debug.WriteLine(TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture).Seconds);
                    timeSpan += (TimeSpan.ParseExact(item.Duration, formats, CultureInfo.InvariantCulture));
                }
            }
            ////Debug.WriteLine(timeSpan);
            var songs = artistAlbums.SelectMany(x => x.Songs).ToList();
            string timeFormat = "";
            CountText.Text = songs.ToList().Count > 1 ? String.Format("MultipleSongs".GetLocalizedString(), songs.Count) : String.Format("SingleSong".GetLocalizedString(), songs.Count);
            if (timeSpan.Hours > 0)
            {
                timeFormat += timeSpan.Hours > 1 ? String.Format("MultipleHours".GetLocalizedString(), timeSpan.Hours) + " " : String.Format("SingleHour".GetLocalizedString(), timeSpan.Hours) + " ";
                //timeFormat += timeSpan.Hours > 1 ? $"{timeSpan.Hours} hours " : $"{timeSpan.Hours} hour ";
            }
            if (timeSpan.Minutes > 0)
            {
                timeFormat += timeSpan.Minutes > 1 ? String.Format("MultipleMinutes".GetLocalizedString(), timeSpan.Minutes) : String.Format("SingleMinute".GetLocalizedString(), timeSpan.Minutes);
            }
            TimeText.Text = timeFormat;

        }
    }
}
