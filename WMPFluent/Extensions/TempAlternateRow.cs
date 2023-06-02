using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace WMPFluent.Extensions
{
    public static partial class ListViewExtensions
    {
        private static Dictionary<IObservableVector<object>, Windows.UI.Xaml.Controls.ListViewBase> _itemsForList = new Dictionary<IObservableVector<object>, Windows.UI.Xaml.Controls.ListViewBase>();

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Brush"/> as an alternate background color to a <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty AlternateColorProperty = DependencyProperty.RegisterAttached("AlternateColor", typeof(Brush), typeof(ListViewExtensions), new PropertyMetadata(null, OnAlternateColorPropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a <see cref="DataTemplate"/> as an alternate template to a <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty AlternateItemTemplateProperty = DependencyProperty.RegisterAttached("AlternateItemTemplate", typeof(DataTemplate), typeof(ListViewExtensions), new PropertyMetadata(null, OnAlternateItemTemplatePropertyChanged));

        /// <summary>
        /// Gets the alternate <see cref="Brush"/> associated with the specified <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to get the associated <see cref="Brush"/> from</param>
        /// <returns>The <see cref="Brush"/> associated with the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></returns>
        public static Brush GetAlternateColor(Windows.UI.Xaml.Controls.ListViewBase obj)
        {
            return (Brush)obj.GetValue(AlternateColorProperty);
        }
        public static GroupStyle GetGroupStyle(DependencyObject obj)
        {
            return (GroupStyle)obj.GetValue(GroupStyleProperty);
        }

        public static void SetGroupStyle(DependencyObject obj, GroupStyle value)
        {
            obj.SetValue(GroupStyleProperty, value);
        }
        public static readonly DependencyProperty GroupStyleProperty =
            DependencyProperty.RegisterAttached("GroupStyle", typeof(ListView), typeof(GroupStyle), new PropertyMetadata(null, GroupStyleChanged));


        public static void GroupStyleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var listview = sender as ListView;

            if (listview == null) return;

            var groupStyle = args.OldValue as GroupStyle;
            if (groupStyle != null)
                listview.GroupStyle.Remove(groupStyle);

            groupStyle = args.NewValue as GroupStyle;
            if (groupStyle != null)
                listview.GroupStyle.Add(groupStyle);
        }
    /// <summary>
    /// Sets the alternate <see cref="Brush"/> associated with the specified <see cref="DependencyObject"/>
    /// </summary>
    /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to associate the <see cref="Brush"/> with</param>
    /// <param name="value">The <see cref="Brush"/> for binding to the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
    public static void SetAlternateColor(Windows.UI.Xaml.Controls.ListViewBase obj, Brush value)
        {
            obj.SetValue(AlternateColorProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="DataTemplate"/> associated with the specified <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to get the associated <see cref="DataTemplate"/> from</param>
        /// <returns>The <see cref="DataTemplate"/> associated with the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></returns>
        public static DataTemplate GetAlternateItemTemplate(Windows.UI.Xaml.Controls.ListViewBase obj)
        {
            return (DataTemplate)obj.GetValue(AlternateItemTemplateProperty);
        }

        /// <summary>
        /// Sets the <see cref="DataTemplate"/> associated with the specified <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to associate the <see cref="DataTemplate"/> with</param>
        /// <param name="value">The <see cref="DataTemplate"/> for binding to the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
        public static void SetAlternateItemTemplate(Windows.UI.Xaml.Controls.ListViewBase obj, DataTemplate value)
        {
            obj.SetValue(AlternateItemTemplateProperty, value);
        }

        private static void OnAlternateColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            listViewBase.ContainerContentChanging -= ColorContainerContentChanging;
            listViewBase.Items.VectorChanged -= ColorItemsVectorChanged;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;

            _itemsForList[listViewBase.Items] = listViewBase;
            if (AlternateColorProperty != null)
            {
                listViewBase.ContainerContentChanging += ColorContainerContentChanging;
                listViewBase.Items.VectorChanged += ColorItemsVectorChanged;
                listViewBase.Unloaded += OnListViewBaseUnloaded;
            }
        }

        private static void ColorContainerContentChanging(Windows.UI.Xaml.Controls.ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var itemContainer = args.ItemContainer as Control;
            SetItemContainerBackground(sender, itemContainer, args.ItemIndex);

            ThemeListener listener = new ThemeListener();
            listener.ThemeChanged += (c) =>
            {
                SetItemContainerBackground(sender, itemContainer, args.ItemIndex);
            };
        }

        private static void OnAlternateItemTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            listViewBase.ContainerContentChanging -= ItemTemplateContainerContentChanging;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;

            if (AlternateItemTemplateProperty != null)
            {
                listViewBase.ContainerContentChanging += ItemTemplateContainerContentChanging;
                listViewBase.Unloaded += OnListViewBaseUnloaded;
            }
        }

        private static void ItemTemplateContainerContentChanging(Windows.UI.Xaml.Controls.ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var itemContainer = args.ItemContainer as SelectorItem;

            if (args.ItemIndex % 2 == 0)
            {
                itemContainer.ContentTemplate = GetAlternateItemTemplate(sender);
            }
            else
            {
                itemContainer.ContentTemplate = sender.ItemTemplate;
            }
        }

        //private static void OnItemContainerStretchDirectionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        //{
        //    Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

        //    if (listViewBase == null)
        //    {
        //        return;
        //    }

        //    listViewBase.ContainerContentChanging -= ItemContainerStretchDirectionChanging;
        //    listViewBase.Unloaded -= OnListViewBaseUnloaded;

        //    if (ItemContainerStretchDirectionProperty != null)
        //    {
        //        listViewBase.ContainerContentChanging += ItemContainerStretchDirectionChanging;
        //        listViewBase.Unloaded += OnListViewBaseUnloaded;
        //    }
        //}

        //private static void ItemContainerStretchDirectionChanging(Windows.UI.Xaml.Controls.ListViewBase sender, ContainerContentChangingEventArgs args)
        //{
        //    var itemContainer = args.ItemContainer as SelectorItem;
        //    var stretchDirection = GetItemContainerStretchDirection(sender);

        //    if (stretchDirection == ItemContainerStretchDirection.Vertical || stretchDirection == ItemContainerStretchDirection.Both)
        //    {
        //        itemContainer.VerticalContentAlignment = VerticalAlignment.Stretch;
        //    }

        //    if (stretchDirection == ItemContainerStretchDirection.Horizontal || stretchDirection == ItemContainerStretchDirection.Both)
        //    {
        //        itemContainer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        //    }
        //}

        private static void OnListViewBaseUnloaded(object sender, RoutedEventArgs e)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;
            _itemsForList.Remove(listViewBase.Items);

            //listViewBase.ContainerContentChanging -= ItemContainerStretchDirectionChanging;
            listViewBase.ContainerContentChanging -= ItemTemplateContainerContentChanging;
            listViewBase.ContainerContentChanging -= ColorContainerContentChanging;
            listViewBase.Items.VectorChanged -= ColorItemsVectorChanged;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;
        }

        private static void ColorItemsVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs args)
        {
            // If the index is at the end we can ignore
            if (args.Index == (sender.Count - 1))
            {
                return;
            }

            // Only need to handle Inserted and Removed because we'll handle everything else in the
            // ColorContainerContentChanging method
            if ((args.CollectionChange == CollectionChange.ItemInserted) || (args.CollectionChange == CollectionChange.ItemRemoved))
            {
                _itemsForList.TryGetValue(sender, out Windows.UI.Xaml.Controls.ListViewBase listViewBase);
                if (listViewBase == null)
                {
                    return;
                }

                int index = (int)args.Index;
                for (int i = index; i < sender.Count; i++)
                {
                    var itemContainer = listViewBase.ContainerFromIndex(i) as Control;
                    if (itemContainer != null)
                    {
                        SetItemContainerBackground(listViewBase, itemContainer, i);
                        //for brushes that are from a ResourceDictionary
                        ThemeListener listener = new ThemeListener();
                        listener.ThemeChanged += (c) =>
                        {
                            SetItemContainerBackground(listViewBase, itemContainer, i);
                        };
                    }
                }
            }
        }

        private static void SetItemContainerBackground(Windows.UI.Xaml.Controls.ListViewBase sender, Control itemContainer, int itemIndex)
        {
            if (itemIndex % 2 == 0)
            {
                itemContainer.Background = GetAlternateColor(sender);
                var rootBorder = itemContainer.FindDescendant<Border>();
                if (rootBorder != null)
                {
                    rootBorder.Background = GetAlternateColor(sender);
                }
            }
            else
            {
                itemContainer.Background = null;

                var rootBorder = itemContainer.FindDescendant<Border>();
                if (rootBorder != null)
                {
                    rootBorder.Background = null;
                }
            }
        }
    }
}
