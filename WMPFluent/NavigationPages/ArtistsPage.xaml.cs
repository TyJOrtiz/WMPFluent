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
            if (ArtistPageViewModel.Template == "Details")
            {
                ArtistView.ItemTemplate = Details;
                MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE179" });
            }
            else if (ArtistPageViewModel.Template == "Icon")
            {
                ArtistView.ItemTemplate = Icon;
                MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE154" });
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.DataTemplateChanged += MainPage_DataTemplateChanged;
            try
            {

                if (ArtistPageViewModel.Template == "Details")
                {
                    ArtistView.ItemTemplate = Details;
                    MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE179" });
                }
                else if (ArtistPageViewModel.Template == "Icon")
                {
                    ArtistView.ItemTemplate = Icon;
                    MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE154" });
                }
            }
            catch
            {

            }
        }

        private void MainPage_DataTemplateChanged(object sender, EventArgs e)
        {
            if (ArtistView.ItemTemplate == Details)
            {
                ArtistView.ItemTemplate = Icon;
                ArtistPageViewModel.Template = "Icon";
                MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE154" });

            }
            else if (ArtistView.ItemTemplate == Icon)
            {
                ArtistView.ItemTemplate = Details;
                ArtistPageViewModel.Template = "Details";
                MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE179" });
            }
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            MainPage.DataTemplateChanged -= MainPage_DataTemplateChanged;
        }
        private void ArtistView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (ArtistView.ItemTemplate == Details)
            {
                try
                {
                    var item = args.ItemContainer.FindDescendant<WMPFluent.Controls.ArtistStackControl>();
                    item.BuildStack(args.Item as LibraryArtist);
                }
                catch
                {

                }
            }
            if (ArtistView.ItemTemplate == Icon)
            {
                try
                {
                    var item = args.ItemContainer.FindDescendant<WMPFluent.Controls.ArtistStackIconControl>();
                    item.BuildStack(args.Item as LibraryArtist);
                }
                catch { }
            }
        }

        private void ArtistView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedViewModel = new SelectedArtistPageViewModel((e.ClickedItem as LibraryArtist).Artist);
            Frame.Navigate(typeof(SelectedArtistPage), selectedViewModel);
        }
    }
}
