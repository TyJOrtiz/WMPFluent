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
using WMPFluent.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WMPFluent.ContentPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectedArtistPage : Page
    {
        private SelectedArtistPageViewModel SelectedArtistPageViewModel;
        public SelectedArtistPage()
        {
            this.InitializeComponent();
            SongView.Loaded += SongView_Loaded;
        }

        private void SongView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ActualWidth < 720)
            {
                (SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement = GroupHeaderPlacement.Top;
            }
            else
            {
                (SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement = GroupHeaderPlacement.Left;
            }
            LayoutChanged1?.Invoke(((SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement == GroupHeaderPlacement.Left), EventArgs.Empty);
            this.SizeChanged += SelectedArtistPage_SizeChanged;
        }

        private void SelectedArtistPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 720)
            {
                (SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement = GroupHeaderPlacement.Top;
            }
            else
            {
                (SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement = GroupHeaderPlacement.Left;
            }
            LayoutChanged1?.Invoke(((SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement == GroupHeaderPlacement.Left), EventArgs.Empty);
        }
        public static event EventHandler LayoutChanged1;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SelectedArtistPageViewModel = (SelectedArtistPageViewModel)e.Parameter;
        }

        private void AdaptiveSongControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ActualWidth < 720)
            {
                (SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement = GroupHeaderPlacement.Top;
            }
            else
            {
                (SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement = GroupHeaderPlacement.Left;
            }
            LayoutChanged1?.Invoke(((SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement == GroupHeaderPlacement.Left), EventArgs.Empty);
        }

        private void ArtistHeaderControl_BringIntoViewRequested(UIElement sender, BringIntoViewRequestedEventArgs args)
        {
            if (this.ActualWidth < 720)
            {
                (SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement = GroupHeaderPlacement.Top;
            }
            else
            {
                (SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement = GroupHeaderPlacement.Left;
            }
            LayoutChanged1?.Invoke(((SongView.ItemsPanelRoot as ItemsStackPanel).GroupHeaderPlacement == GroupHeaderPlacement.Left), EventArgs.Empty);
        }
    }
}
