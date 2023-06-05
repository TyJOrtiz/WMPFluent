using Microsoft.Toolkit.Uwp.UI;
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
using WMPFluent.ContentPages;
using WMPFluent.Models;
using WMPFluent.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WMPFluent.NavigationPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtistsPage : Page
    {
        private ArtistPageViewModel ArtistPageViewModel;
        public ArtistsPage()
        {
            this.InitializeComponent();
            this.ArtistPageViewModel = new ArtistPageViewModel();
        }
        private void ArtistView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var item = args.ItemContainer.FindDescendant<WMPFluent.Controls.ArtistStackControl>();
            item.BuildStack(args.Item as LibraryArtist);
        }

        private void ArtistView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedViewModel = new SelectedArtistPageViewModel((e.ClickedItem as LibraryArtist).Artist);
            Frame.Navigate(typeof(SelectedArtistPage), selectedViewModel);
        }
    }
}
