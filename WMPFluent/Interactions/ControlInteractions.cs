using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.UI;
using System.Diagnostics;
using Windows.Graphics.Imaging;
using Windows.Storage;
using ColorThiefDotNet;
using Windows.UI;
using Color = Windows.UI.Color;
using Prism.Commands;
//using WMPFluent.Controls;
using WMPFluent.Extensions;
using WMPFluent.Models;

namespace WMPFluent.Interactions
{
    public class ControlInteractions
    {

        public static async void GetAlbumArtColor()
        {
            try
            {
                //var file = await StorageFile.GetFileFromPathAsync(App.AppViewModel.PlayerControls.CurrentPlaybackItem.Source.CustomProperties["AlbumArt"] as string);
                //var stream = await file.OpenAsync(FileAccessMode.Read);
                //var decoder = await BitmapDecoder.CreateAsync(stream);
                //ColorThief colorThief = new ColorThief();
                //var pallete = await colorThief.GetPalette(decoder, 5, 5, true);
                //pallete = pallete.OrderByDescending(x => x.Population).ToList();

                //Color color1 = new Windows.UI.Color
                //{
                //    A = pallete[0].Color.A,
                //    B = pallete[0].Color.B,
                //    G = pallete[0].Color.G,
                //    R = pallete[0].Color.R
                //};
                //Color color2 = Colors.Black;
                //double amount = 0.5;
                //Color lerpColor = LerpColor(color1, color2, amount);
                ////Debug.WriteLine(IsColorCloseToBlack(lerpColor));
                //if (IsColorCloseToBlack(lerpColor))
                //{
                //    color1 = new Windows.UI.Color
                //    {
                //        A = pallete[1].Color.A,
                //        B = pallete[1].Color.B,
                //        G = pallete[1].Color.G,
                //        R = pallete[1].Color.R
                //    };
                //    color2 = Colors.Black;
                //    amount = 0.5;
                //    lerpColor = LerpColor(color1, color2, amount);
                //}
                ////App.AppViewModel.PlayerControls.backingSheet.Background = new SolidColorBrush(lerpColor);
            }
            catch
            {

            }
        }
        private const byte BlackThreshold = 50;

        private static bool IsColorCloseToBlack(Color color)
        {
            return color.R < BlackThreshold && color.G < BlackThreshold && color.B < BlackThreshold;
        }
        private static Color LerpColor(Color color1, Color color2, double amount)
        {
            byte r = (byte)(color1.R + (color2.R - color1.R) * amount);
            byte g = (byte)(color1.G + (color2.G - color1.G) * amount);
            byte b = (byte)(color1.B + (color2.B - color1.B) * amount);
            byte a = (byte)(color1.A + (color2.A - color1.A) * amount);
            return Color.FromArgb(a, r, g, b);
        }
        public static DelegateCommand CallPlaylistView(dynamic objectToadd = null)
        {
            return new DelegateCommand(new Action(async () =>
            {
                //var dialog = new PlaylistDialog();
                //dialog.DataContext = objectToadd;
                //await dialog.ShowAsync();
            }));
        }
        private static event EventHandler<ScrollViewerViewChangingEventArgs> primaryhandler;
        public static void RegisterScrollInteration(Control rootScroll, Grid header, bool toggleTransparency)
        {
            header.TranslationTransition = new Windows.UI.Xaml.Vector3Transition
            {
                Duration = TimeSpan.FromMilliseconds(300),
                Components = Windows.UI.Xaml.Vector3TransitionComponents.Z
            };
            if (toggleTransparency)
            {
                header.OpacityTransition = new ScalarTransition
                {
                    Duration = TimeSpan.FromMilliseconds(300)
                };
            }
            if (rootScroll.GetType() == typeof(ScrollViewer)) 
            {
                primaryhandler = (s, e) =>
                {
                    //Debug.WriteLine(e.FinalView.VerticalOffset);
                    if (e.FinalView.VerticalOffset > 0)
                    {
                        header.Translation = new System.Numerics.Vector3(0, 0, 8);
                        if (toggleTransparency)
                        {
                            header.Opacity = 1;
                        }
                    }
                    else
                    {
                        header.Translation = new System.Numerics.Vector3(0);
                        if (toggleTransparency)
                        {
                            header.Opacity = 0;
                        }
                    }
                };
                (rootScroll as ScrollViewer).ViewChanging += primaryhandler;
            }
            //else if (rootScroll.GetType() == typeof(ListView) || rootScroll.GetType() == typeof(GridView) || rootScroll.GetType() == typeof(Controls.CustomAdaptiveGridView))
            {
                primaryhandler = (s, e) =>
                {
                    //Debug.WriteLine(e.FinalView.VerticalOffset);
                    if (e.FinalView.VerticalOffset > 0)
                    {
                        header.Translation = new System.Numerics.Vector3(0, 0, 8);
                        if (toggleTransparency)
                        {
                            header.Opacity = 1;
                        }
                    }
                    else
                    {
                        header.Translation = new System.Numerics.Vector3(0);
                        if (toggleTransparency)
                        {
                            header.Opacity = 0;
                        }
                    }
                };
                var scrollViewer = ((UIElement)rootScroll).FindDescendant<ScrollViewer>();
                scrollViewer.ViewChanging += primaryhandler;

                //(s, e) =>
                //{
                //    //Debug.WriteLine(e.FinalView.VerticalOffset);
                //    if (e.FinalView.VerticalOffset > 0)
                //    {
                //        header.Translation = new System.Numerics.Vector3(0, 0, 8);
                //        if (toggleTransparency)
                //        {
                //            header.Opacity = 1;
                //        }
                //    }
                //    else
                //    {
                //        header.Translation = new System.Numerics.Vector3(0);
                //        if (toggleTransparency)
                //        {
                //            header.Opacity = 0;
                //        }
                //    }
                //};
            }
        }

        internal static void RegisterCarouselInteraction(Control rootScroll, Button leftButton, Button rightButton)
        {
            ScrollViewer scroll = null;
            if (rootScroll.GetType() == typeof(ScrollViewer))
            {
                scroll = (rootScroll as ScrollViewer);
            }
            //else if (rootScroll.GetType() == typeof(ListView) || rootScroll.GetType() == typeof(GridView) || rootScroll.GetType() == typeof(Controls.CustomAdaptiveGridView))
            {

                scroll = ((UIElement)rootScroll).FindDescendant<ScrollViewer>();
            }
            if (scroll != null)
            {
                if (scroll.HorizontalOffset == 0)
                {
                    leftButton.Visibility = Visibility.Collapsed;
                }
                if (scroll.HorizontalOffset + scroll.ActualWidth < scroll.ExtentWidth)
                {
                    rightButton.Visibility = Visibility.Visible;
                }
                else
                {
                    rightButton.Visibility = Visibility.Collapsed;
                }
                scroll.SizeChanged += (z, c) =>
                {
                    if (scroll.HorizontalOffset == 0) //vertical offset 0 => at the top of the scrollviewer
                    {
                        leftButton.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        leftButton.Visibility = Visibility.Visible;
                    }
                    if (scroll.HorizontalOffset +
                        scroll.ActualWidth < scroll.ExtentWidth)
                    {
                        rightButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        rightButton.Visibility = Visibility.Collapsed;
                    }
                };
                scroll.ViewChanged += (s, c) =>
                {
                    if (scroll.HorizontalOffset == 0) //vertical offset 0 => at the top of the scrollviewer
                    {
                        leftButton.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        leftButton.Visibility = Visibility.Visible;
                    }
                    if (scroll.HorizontalOffset +
                        scroll.ActualWidth < scroll.ExtentWidth)
                    {
                        rightButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        rightButton.Visibility = Visibility.Collapsed;
                    }
                };
                leftButton.Click += (l, b) =>
                {
                    scroll.ChangeView(scroll.HorizontalOffset - scroll.ViewportWidth, null, null);
                };
                rightButton.Click += (l, b) =>
                {
                    scroll.ChangeView(scroll.HorizontalOffset + scroll.ViewportWidth, null, null);
                };
            }
        }

        internal static void RegisterSecondaryScrollInteration(Control rootScroll, Grid header, bool toggleTransparency, double height)
        {
            var margin = new Thickness(0);
            header.IsHitTestVisible = true;
            //header.TranslationTransition = new Windows.UI.Xaml.Vector3Transition
            //{
            //    Duration = TimeSpan.FromMilliseconds(300),
            //    Components = Windows.UI.Xaml.Vector3TransitionComponents.Z
            //};
            //if (toggleTransparency)
            //{
            //    header.OpacityTransition = new ScalarTransition
            //    {
            //        Duration = TimeSpan.FromMilliseconds(300)
            //    };
            //}
            if (rootScroll.GetType() == typeof(ScrollViewer))
            {
                var scrollViewer = (ScrollViewer)rootScroll;
                margin = Microsoft.Toolkit.Uwp.UI.ScrollViewerExtensions.GetVerticalScrollBarMargin(scrollViewer);
                var tophome = margin.Top;
                var topscrolled = margin.Top + 48;
                (rootScroll as ScrollViewer).ViewChanging += (s, e) =>
                {
                    //Debug.WriteLine(e.FinalView.VerticalOffset);
                    if (e.FinalView.VerticalOffset >= height)
                    {
                        if (toggleTransparency)
                        {
                            header.Visibility = Visibility.Visible;
                            margin.Top = topscrolled;
                            Microsoft.Toolkit.Uwp.UI.ScrollViewerExtensions.SetVerticalScrollBarMargin(scrollViewer, margin);
                        }
                    }
                    else
                    {
                        if (toggleTransparency)
                        {
                            header.Visibility = Visibility.Collapsed;
                            margin.Top = tophome;
                            Microsoft.Toolkit.Uwp.UI.ScrollViewerExtensions.SetVerticalScrollBarMargin(scrollViewer, margin);
                        }
                    }
                };
            }
            //else if (rootScroll.GetType() == typeof(ListView) || rootScroll.GetType() == typeof(GridView) || rootScroll.GetType() == typeof(Controls.CustomAdaptiveGridView))
            {

                var scrollViewer = ((UIElement)rootScroll).FindDescendant<ScrollViewer>();
                margin = Microsoft.Toolkit.Uwp.UI.ScrollViewerExtensions.GetVerticalScrollBarMargin(scrollViewer); 
                var tophome = -48;
                var topscrolled = 0;
                scrollViewer.ViewChanging += (s, e) =>
                {
                    //Debug.WriteLine(e.FinalView.VerticalOffset);
                    if (e.FinalView.VerticalOffset >= height)
                    {
                        if (toggleTransparency)
                        {
                            header.Visibility = Visibility.Visible;
                            margin.Top = topscrolled;
                            Microsoft.Toolkit.Uwp.UI.ScrollViewerExtensions.SetVerticalScrollBarMargin(scrollViewer, margin);
                        }
                    }
                    else
                    {
                        if (toggleTransparency)
                        {
                            header.Visibility = Visibility.Collapsed;
                            margin.Top = tophome;
                            Microsoft.Toolkit.Uwp.UI.ScrollViewerExtensions.SetVerticalScrollBarMargin(scrollViewer, margin);
                        }
                    }
                };
            }
        }

        internal static void ReregisterScrollInteration(Control rootScroll, Grid header, bool toggleTransparency)
        {
            if (rootScroll.GetType() == typeof(ScrollViewer))
            {
                (rootScroll as ScrollViewer).ViewChanging -= primaryhandler;
            }
            //else if (rootScroll.GetType() == typeof(ListView) || rootScroll.GetType() == typeof(GridView) || rootScroll.GetType() == typeof(Controls.CustomAdaptiveGridView))
            {
                var scrollViewer = ((UIElement)rootScroll).FindDescendant<ScrollViewer>();
                scrollViewer.ViewChanging -= primaryhandler;
            }
            RegisterScrollInteration(rootScroll, header, toggleTransparency);
        }
    }
}
