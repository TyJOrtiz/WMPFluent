using CommunityToolkit.Labs.WinUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WMPFluent.Models;
using WMPFluent.NavigationPages;
using WMPFluent.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WMPFluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<NavigationObject> NavigationObjects = new ObservableCollection<NavigationObject>
        {
            new NavigationObject
            {
                Name = "Playlists",
                Items = new ObservableCollection<object>
                {

                },
                Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE142" },
            },
            new NavigationObject
            {
                Name = "Library",
                Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE1D3" },
                IsSelectEnabled = false,
                Page = "WMPFluent.NavigationPages.LibraryPage",
                Items = new ObservableCollection<object>
                {
                    new NavigationObject
                    {
                        Name = "Artists",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE125" },
                        Page = "WMPFluent.NavigationPages.ArtistsPage"
                    },
                    new NavigationObject
                    {
                        Name = "Albums",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE93C" },
                        Page = "WMPFluent.NavigationPages.AlbumsPage"
                    },
                    new NavigationObject
                    {
                        Name = "Songs",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uEC4F" },
                        Page = "WMPFluent.NavigationPages.SongsPage"
                    },
                    new NavigationObject
                    {
                        Name = "Genres",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE74C" },
                        Page = "WMPFluent.NavigationPages.GenresPage"
                    },
                    new NavigationObject
                    {
                        Name = "Folders",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE8B7" },
                        Page = "WMPFluent.NavigationPages.FoldersPage"
                    },

                }
            }
        };
        private NavigationObject PlaylistNode;
        private SetupViewModel SetupViewModel;
        private static event EventHandler ViewChanged1;
        public MainPage()
        {
            this.InitializeComponent();
            ViewChanged1 += MainPage_ViewChanged1;
            this.SizeChanged += MainPage_SizeChanged;
            PlaylistNode = NavigationObjects.First();
            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;
            var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            appTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            ContentFrame.Navigated += ContentFrame_Navigated;
            if (App.AppViewModel.IsMediaAvailable == false)
            {
                Filecount.Visibility = Visibility.Visible;
                SetupViewModel = new SetupViewModel();
            }
            else
            {
                ContentFrame.Navigate(typeof(NavigationPages.AlbumsPage));
                App.AppViewModel.BreadcrumbNavigationItems = new ObservableCollection<object>
                {
                    "Library",
                    "Albums",
                };
                App.AppViewModel.HookMedia(MediaHost, Transport);
            }
            App.AppViewModel.RootFrame = ContentFrame;
        }

        private void MainPage_ViewChanged1(object sender, EventArgs e)
        {
            TemplateSelector.Content = sender as FontIcon;
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine(e.NewSize.Width);
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            try
            {
                NavList.SelectedItem = NavigationObjects.Last().Items.FirstOrDefault(x => ((NavigationObject)x).Page == e.SourcePageType.FullName);
                App.AppViewModel.BreadcrumbNavigationItems = new ObservableCollection<object>
                {
                    "Library",
                    (NavList.SelectedItem as NavigationObject).Name
                };
                //NavList.SelectedItem = NavigationObjects.FirstOrDefault(x => x.Page == e.SourcePageType.FullName);
            }
            catch
            {
                if (e.Parameter != null)
                {
                    if (e.Parameter is SelectedAlbumViewModel selectedAlbumViewModel)
                    {
                        App.AppViewModel.BreadcrumbNavigationItems = new ObservableCollection<object>
                        {
                            "Library",
                            "Albums",
                            selectedAlbumViewModel.Album.Name
                        };
                    }
                    else if (e.Parameter is SelectedArtistPageViewModel selectedArtistPageViewModel)
                    {
                        App.AppViewModel.BreadcrumbNavigationItems = new ObservableCollection<object>
                        {
                            "Library",
                            "Artists",
                            selectedArtistPageViewModel.Artist
                        };
                    }
                }
                try
                {
                    NavList.SelectedItem = null;
                }
                catch
                {

                }
            }
        }

        private void PlayList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen || e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                if (((FrameworkElement)e.OriginalSource).DataContext is NowPlayingItem npi)
                {
                    App.AppViewModel.MediaPlaybackList.MoveTo((uint)App.AppViewModel.MediaPlaybackList.Items.IndexOf(npi.MediaPlaybackItem));
                }
            }
        }

        private void PlayList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
            {
                if (((FrameworkElement)e.OriginalSource).DataContext is NowPlayingItem npi)
                {
                    App.AppViewModel.MediaPlaybackList.MoveTo((uint)App.AppViewModel.MediaPlaybackList.Items.IndexOf(npi.MediaPlaybackItem));
                }
            }
        }
        private void Segmented_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (((Segmented)sender).SelectedItem == e.ClickedItem)
            {
                SideBar.IsPaneOpen = false;
            }
            else
            {
                SideBar.IsPaneOpen = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.GoBack();
        }

        private void NavList_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            try
            {
                var object1 = args.InvokedItemContainer.DataContext as NavigationObject; if (object1 != null)
                {
                    var type = Type.GetType(object1.Page);
                    ContentFrame.NavigateToType(type, null, null);
                }
            }
            catch
            {

            }
        }
        public static event EventHandler DataTemplateChanged;
        private void SplitButton_Click(Microsoft.UI.Xaml.Controls.SplitButton sender, Microsoft.UI.Xaml.Controls.SplitButtonClickEventArgs args)
        {
            DataTemplateChanged?.Invoke(null, null);
        }

        internal static void UpdateViewIcon(FontIcon fontIcon)
        {
            ViewChanged1?.Invoke(fontIcon, null);
        }
    }
}
