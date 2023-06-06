using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
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
using WMPFluent.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WMPFluent.Controls
{
    public sealed partial class ArtistStackIconControl : UserControl
    {
        public LibraryArtist artist { get { return this.DataContext as LibraryArtist; } }
        public ArtistStackIconControl()
        {
            this.InitializeComponent();
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
                        Height = 89.1,
                        Width = 89.1,
                        Opacity = opacity,
                        CenterPoint = new System.Numerics.Vector3((float)44.55, (float)44.55, 0),
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
        }
    }
}
