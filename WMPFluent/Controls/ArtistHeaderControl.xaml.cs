using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WMPFluent.Controls
{
    public sealed partial class ArtistHeaderControl : UserControl
    {
        private bool showAdaptive;

        public LibraryAlbum album { get { return this.DataContext as LibraryAlbum; } }
        public ArtistHeaderControl()
        {
            this.InitializeComponent();
            MainGrid.Visibility = showAdaptive ? Visibility.Visible : Visibility.Collapsed;
            CompactGrid.Visibility = showAdaptive ? Visibility.Collapsed : Visibility.Visible;
            SelectedArtistPage.LayoutChanged1 += SelectedArtistPage_LayoutChanged1;
            this.DataContextChanged += (s, e) => this.Bindings.Update();
            HostFrame = App.AppViewModel.RootFrame;
            Debug.WriteLine(HostFrame.ActualWidth);

            if (showAdaptive)
            {
                if (HostFrame.ActualWidth >= 0 && HostFrame.ActualWidth <= 540)
                {
                    TextDetails.Visibility = Visibility.Collapsed;
                    MainGrid.Width = HostFrame.ActualWidth * .14;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                }
                if (HostFrame.ActualWidth <= 1080 && HostFrame.ActualWidth > 540)
                {
                    TextDetails.Visibility = Visibility.Visible;
                    MainGrid.Width = (HostFrame.ActualWidth * .14) + 150;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(HostFrame.ActualWidth * .14, GridUnitType.Pixel);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(150, GridUnitType.Pixel);
                }
                else
                {
                    if (HostFrame.ActualWidth > 1080)
                    {
                        TextDetails.Visibility = Visibility.Visible;
                        MainGrid.Width = 320;
                        MainGrid.ColumnDefinitions[0].Width = new GridLength(150, GridUnitType.Pixel);
                        MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    }
                }
            }
            HostFrame.SizeChanged += HostFrame_SizeChanged;
        }
        public void turnonAdaptive()
        {
            showAdaptive = true;
            MainGrid.Visibility = showAdaptive ? Visibility.Visible : Visibility.Collapsed;
            CompactGrid.Visibility = showAdaptive ? Visibility.Collapsed : Visibility.Visible;
            if (showAdaptive)
            {
                if (HostFrame.ActualWidth >= 0 && HostFrame.ActualWidth <= 540)
                {
                    TextDetails.Visibility = Visibility.Collapsed;
                    MainGrid.Width = HostFrame.ActualWidth * .14;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                }
                if (HostFrame.ActualWidth <= 1080 && HostFrame.ActualWidth > 540)
                {
                    TextDetails.Visibility = Visibility.Visible;
                    MainGrid.Width = (HostFrame.ActualWidth * .14) + 150;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(HostFrame.ActualWidth * .14, GridUnitType.Pixel);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(150, GridUnitType.Pixel);
                }
                else
                {
                    if (HostFrame.ActualWidth > 1080)
                    {
                        TextDetails.Visibility = Visibility.Visible;
                        MainGrid.Width = 320;
                        MainGrid.ColumnDefinitions[0].Width = new GridLength(150, GridUnitType.Pixel);
                        MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    }
                }
            }
        }
        private void SelectedArtistPage_LayoutChanged1(object sender, EventArgs e)
        {
            showAdaptive = (bool)sender;
            MainGrid.Visibility = showAdaptive ? Visibility.Visible : Visibility.Collapsed;
            CompactGrid.Visibility = showAdaptive ? Visibility.Collapsed : Visibility.Visible;
            if (showAdaptive)
            {
                if (HostFrame.ActualWidth >= 0 && HostFrame.ActualWidth <= 540)
                {
                    TextDetails.Visibility = Visibility.Collapsed;
                    MainGrid.Width = HostFrame.ActualWidth * .14;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                }
                if (HostFrame.ActualWidth <= 1080 && HostFrame.ActualWidth > 540)
                {
                    TextDetails.Visibility = Visibility.Visible;
                    MainGrid.Width = (HostFrame.ActualWidth * .14) + 150;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(HostFrame.ActualWidth * .14, GridUnitType.Pixel);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(150, GridUnitType.Pixel);
                }
                else
                {
                    if (HostFrame.ActualWidth > 1080)
                    {
                        TextDetails.Visibility = Visibility.Visible;
                        MainGrid.Width = 320;
                        MainGrid.ColumnDefinitions[0].Width = new GridLength(150, GridUnitType.Pixel);
                        MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    }
                }
            }
        }

        private void HostFrame_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine(e.NewSize.Width);
            if (showAdaptive)
            {
                if (e.NewSize.Width >= 0 && e.NewSize.Width <= 540)
                {
                    TextDetails.Visibility = Visibility.Collapsed;
                    MainGrid.Width = e.NewSize.Width * .14;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
                }
                if (e.NewSize.Width <= 1080 && e.NewSize.Width > 540)
                {
                    TextDetails.Visibility = Visibility.Visible;
                    MainGrid.Width = (e.NewSize.Width * .14) + 150;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(e.NewSize.Width * .14, GridUnitType.Pixel);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(150, GridUnitType.Pixel);
                }
                else
                {
                    if (e.NewSize.Width > 1080)
                    {
                        TextDetails.Visibility = Visibility.Visible;
                        MainGrid.Width = 320;
                        MainGrid.ColumnDefinitions[0].Width = new GridLength(150, GridUnitType.Pixel);
                        MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    }
                }
            }
            //if (e.NewSize.Width <= 360)
            //{
            //    TextDetails.Visibility = Visibility.Collapsed;
            //    MainGrid.Width = e.NewSize.Width - (e.NewSize.Width - 48);
            //}
            //else if (e.NewSize.Width < 720 && e.NewSize.Width > 360)
            //{
            //    TextDetails.Visibility = Visibility.Collapsed;
            //    MainGrid.Width = e.NewSize.Width * .2;
            //}
            //else
            //{
            //    TextDetails.Visibility = Visibility.Visible;
            //    MainGrid.Width = 310;
            //}
        }

        public Frame HostFrame { get; private set; }

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

        private void ImageEx_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height < 80)
            {
                YearText.Visibility = Visibility.Collapsed;
            }
            else
            {
                YearText.Visibility = Visibility.Visible;
            }
        }
    }
}
