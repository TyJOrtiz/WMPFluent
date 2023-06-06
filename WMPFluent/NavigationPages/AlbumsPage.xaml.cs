using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class AlbumsPage : Page
    {
        private AlbumPageViewModel AlbumPageViewModel;
        public AlbumsPage()
        {
            this.InitializeComponent();
            this.AlbumPageViewModel = new AlbumPageViewModel();
            if (AlbumPageViewModel.Template == "Details")
            {
                AlbumView.ItemTemplate = Details;
                MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE179" });
            }
            else if (AlbumPageViewModel.Template == "Icon")
            {
                AlbumView.ItemTemplate = Icon;
                MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE154" });
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.DataTemplateChanged += MainPage_DataTemplateChanged;
            try
            {

                if (AlbumPageViewModel.Template == "Details")
                {
                    AlbumView.ItemTemplate = Details;
                    MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE179" });
                }
                else if (AlbumPageViewModel.Template == "Icon")
                {
                    AlbumView.ItemTemplate = Icon;
                    MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE154" });
                }
            }
            catch
            {

            }
        }

        private void MainPage_DataTemplateChanged(object sender, EventArgs e)
        {
            if (AlbumView.ItemTemplate == Details)
            {
                AlbumView.ItemTemplate = Icon;
                AlbumPageViewModel.Template = "Icon";
                MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE154" });

            }
            else if (AlbumView.ItemTemplate == Icon)
            {
                AlbumView.ItemTemplate = Details;
                AlbumPageViewModel.Template = "Details";
                MainPage.UpdateViewIcon(new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"), FontSize = 16, Glyph = "\uE179" });
            }
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            MainPage.DataTemplateChanged -= MainPage_DataTemplateChanged;
        }
        private void AlbumView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedAlbumViewModel = new SelectedAlbumViewModel(e.ClickedItem as LibraryAlbum);
            Frame.Navigate(typeof(SelectedAlbumPage), selectedAlbumViewModel);
        }
    }
}
