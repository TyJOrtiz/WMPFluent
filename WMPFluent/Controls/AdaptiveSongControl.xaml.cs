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
    public sealed partial class AdaptiveSongControl : UserControl
    {
        public LibrarySong song { get { return this.DataContext as LibrarySong; } }
        public AdaptiveSongControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => this.Bindings.Update();
            UpdateLayout();
            UpdateControlLayout(this.ActualWidth);
            this.SizeChanged += AdaptiveSongControl_SizeChanged;
        }
        
        private void AdaptiveSongControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateControlLayout(e.NewSize.Width);
        }

        private void UpdateControlLayout(double width)
        {
            try
            {
                MainGrid.Width = width - 10;
            }
            catch
            {

            }
            if (width < 648)
            {
                MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                MainGrid.ColumnDefinitions[3].Width = new GridLength(48, GridUnitType.Pixel);
            }
            else if (width >= 648 && width < 720)
            {
                MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Pixel);
                MainGrid.ColumnDefinitions[3].Width = new GridLength(48, GridUnitType.Pixel);
            }
            else if (width >= 720)
            {
                MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[3].Width = new GridLength(48, GridUnitType.Pixel);
            }
        }
    }
}
