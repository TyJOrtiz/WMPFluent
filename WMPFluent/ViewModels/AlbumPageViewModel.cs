using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WMPFluent.Extensions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Microsoft.Toolkit.Collections;
using System.Collections.ObjectModel;
using WMPFluent.Models;
using Windows.Storage;
using Newtonsoft.Json.Linq;

namespace WMPFluent.ViewModels
{
    public class AlbumPageViewModel : ContentViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<SortOption> SortValues = new ObservableCollection<SortOption>
        {
            new SortOption
            {
                FriendlyName = "A-Z".GetLocalizedString(),
                Value = "A-Z",
                FirstSortOption = "Name",
                SecondSortOption =  "ArtistName"
            },
            new SortOption
            {
                FriendlyName = "Artist".GetLocalizedString(),
                Value = "Artist",
                FirstSortOption = "ArtistName",
                SecondSortOption =  "Name"
            },
            new SortOption
            {
                FriendlyName = "ReleaseYearAscending".GetLocalizedString(), 
                Value = "ReleaseYearAscending",
                FirstSortOption = "Year",
                SecondSortOption =  "Name"
            },
            new SortOption
            {
                FriendlyName = "ReleaseYearDescending".GetLocalizedString(), 
                Value = "ReleaseYearDescending",
                FirstSortOption = "Year",
                SecondSortOption =  "Name"
            },
            new SortOption
            {
                FriendlyName = "DateAdded".GetLocalizedString(), 
                Value = "DateAdded",
                FirstSortOption = "DateAdded",
                SecondSortOption =  "Name"
            },

        };
        private SortOption selectedSortOption;
        public SortOption SelectedSortOption
        {
            get
            {
                return selectedSortOption;
            }
            set
            {
                if (selectedSortOption != value)
                {
                    selectedSortOption = value;
                    NotifyPropertyChanged();
                    ApplicationData.Current.LocalSettings.Values["AlbumSortOption"] = value.Value;
                    Sort(value);
                }
            }
        }

        private void Sort(SortOption value)
        {
            LibraryAlbums.Clear();
            if (value.Value == "A-Z" || value.Value == "Artist")
            {
                var d = from a in App.AppViewModel.LibraryAlbums.OrderBy(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(value.FirstSortOption).GetValue(a, null).ToString().ToUpper().First().CheckChar().DeAccent();
                this.AlbumSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = true,
                    Source = this.AlbumSource,
                };
                LibraryAlbums.AddRange(App.AppViewModel.LibraryAlbums.OrderBy(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()));
            }
            else if (value.Value == "ReleaseYearAscending")
            {
                var d = from a in App.AppViewModel.LibraryAlbums.OrderBy(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(value.FirstSortOption).GetValue(a, null).ToString();
                this.AlbumSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = true,
                    Source = this.AlbumSource,
                };
                LibraryAlbums.AddRange(App.AppViewModel.LibraryAlbums.OrderBy(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()));
            }
            else if (value.Value == "ReleaseYearDescending")
            {
                var d = from a in App.AppViewModel.LibraryAlbums.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(value.FirstSortOption).GetValue(a, null).ToString();
                this.AlbumSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = true,
                    Source = this.AlbumSource,
                };
                LibraryAlbums.AddRange(App.AppViewModel.LibraryAlbums.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()));
            }
            else if (value.Value == "DateAdded")
            {
                var d = from a in App.AppViewModel.LibraryAlbums.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(value.FirstSortOption).GetValue(a, null).ToString();
                this.AlbumSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = false,
                    Source = App.AppViewModel.LibraryAlbums.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()),
                };
                LibraryAlbums.AddRange(App.AppViewModel.LibraryAlbums.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()));
            }
        }
        private ObservableCollection<LibraryAlbum> libraryAlbums;
        public ObservableCollection<LibraryAlbum> LibraryAlbums 
        { 
            get { 
                return libraryAlbums; 
            }
            set {
                libraryAlbums = value;
                NotifyPropertyChanged();
            }
        }
        public AlbumPageViewModel() 
        {

            LibraryAlbums = new ObservableCollection<LibraryAlbum>();
            if (ApplicationData.Current.LocalSettings.Values["AlbumSortOption"] != null)
            {
                try
                {
                    SelectedSortOption = SortValues.FirstOrDefault(x => x.Value == (string)ApplicationData.Current.LocalSettings.Values["AlbumSortOption"]);
                }
                catch
                {
                    SelectedSortOption = SortValues.FirstOrDefault();
                }
            }
            else
            {
                SelectedSortOption = SortValues.First();
            }
            if (ApplicationData.Current.LocalSettings.Values["AlbumItemTemplate"] != null)
            {
                Template = (string)ApplicationData.Current.LocalSettings.Values["AlbumItemTemplate"];
                
            }
            else
            {
                Template = "Details";
            }
        }
        private string template;
        public string Template
        {
            get
            {
                return template;
            }
            set
            {
                template = value;
                ApplicationData.Current.LocalSettings.Values["AlbumItemTemplate"] = value;
                NotifyPropertyChanged();
            }
        }
        private ObservableCollection<string> genres;
        public ObservableCollection<string> Genres
        {
            get
            {
                return genres;
            }
            set
            {
                if (genres != value)
                {
                    genres = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private CollectionViewSource collectionView;
        public CollectionViewSource CollectionView
        {
            get { return collectionView; }
            set
            {
                collectionView = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableGroupedCollection<string, object> albumSource;

        public ObservableGroupedCollection<string, object> AlbumSource
        {
            get { return albumSource; }
            set
            {
                albumSource = value;
                NotifyPropertyChanged();
            }
        }

        private void LoadSortedView()
        {
            

        }

        internal void FilterQuery(string text)
        {
            if (text.Length > 0)
            {
                if (selectedSortOption.Value == "A-Z" || selectedSortOption.Value == "Artist")
                {
                    var d = from a in App.AppViewModel.LibraryAlbums.Where(x => x.Name.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase) || x.ArtistName.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x.GetType().GetProperty(selectedSortOption.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(selectedSortOption.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(selectedSortOption.FirstSortOption).GetValue(a, null).ToString().ToUpper().First().CheckChar().DeAccent();
                    this.AlbumSource = new ObservableGroupedCollection<string, object>(d);
                    this.CollectionView = new CollectionViewSource
                    {
                        IsSourceGrouped = true,
                        Source = this.AlbumSource,
                    };
                }
                if (selectedSortOption.Value == "Release year")
                {
                    var d = from a in App.AppViewModel.LibraryAlbums.Where(x => x.Name.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase) || x.ArtistName.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.GetType().GetProperty(selectedSortOption.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(selectedSortOption.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(selectedSortOption.FirstSortOption).GetValue(a, null).ToString();
                    this.AlbumSource = new ObservableGroupedCollection<string, object>(d);
                    this.CollectionView = new CollectionViewSource
                    {
                        IsSourceGrouped = true,
                        Source = this.AlbumSource,
                    };
                }
                else if (selectedSortOption.Value == "Date added")
                {
                    var d = from a in App.AppViewModel.LibraryAlbums.Where(x => x.Name.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase) || x.ArtistName.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.GetType().GetProperty(selectedSortOption.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(selectedSortOption.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(selectedSortOption.FirstSortOption).GetValue(a, null).ToString();
                    this.AlbumSource = new ObservableGroupedCollection<string, object>(d);
                    this.CollectionView = new CollectionViewSource
                    {
                        IsSourceGrouped = false,
                        Source = App.AppViewModel.LibraryAlbums.Where(x => x.Name.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase) || x.ArtistName.Contains(text.DeAccent().DePunctuate(), StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.GetType().GetProperty(selectedSortOption.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(selectedSortOption.SecondSortOption).GetValue(x, null).ToString()),
                    };
                }
            }
            else
            {
                Sort(SelectedSortOption);
            }
        }
    }
}
