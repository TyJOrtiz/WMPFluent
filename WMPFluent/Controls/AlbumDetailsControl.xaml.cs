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
    public sealed partial class AlbumDetailsControl : UserControl
    {
        public LibraryAlbum album { get { return this.DataContext as LibraryAlbum; } }
        public AlbumDetailsControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => this.Bindings.Update();
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen || e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                PlayButton.Visibility = Visibility.Visible;
            }
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen || e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                PlayButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
