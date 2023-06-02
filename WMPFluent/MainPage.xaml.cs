using CommunityToolkit.Labs.WinUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                Items = new ObservableCollection<object>
                {
                    new NavigationObject
                    {
                        Name = "Artists",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE125" }
                    },
                    new NavigationObject
                    {
                        Name = "Albums",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE93C" }
                    },
                    new NavigationObject
                    {
                        Name = "Songs",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uEC4F" }
                    },
                    new NavigationObject
                    {
                        Name = "Genres",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE74C" }
                    },
                    new NavigationObject
                    {
                        Name = "Folders",
                        Icon = new FontIcon { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons") , Glyph = "\uE8B7" }
                    },

                }
            }
        };
        private NavigationObject PlaylistNode;
        private SetupViewModel SetupViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            PlaylistNode = NavigationObjects.First(); 
            if (App.AppViewModel.IsMediaAvailable == false)
            {
                SetupViewModel = new SetupViewModel();
            }
            ContentFrame.Navigate(typeof(NavigationPages.AlbumsPage));
            App.AppViewModel.BreadcrumbNavigationItems = new ObservableCollection<object>
            {
                "Library",
                "Albums",
            };
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
    }
}
