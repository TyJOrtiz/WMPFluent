using Microsoft.Toolkit.Collections;
using WMPFluent.Extensions;
using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace WMPFluent.ViewModels
{
    internal class SongPageViewModel : INotifyPropertyChanged
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
                Value = "A-Z",
                FriendlyName = "A-Z".GetLocalizedString(),
                FirstSortOption = "Name",
                SecondSortOption =  "SongArtistName"
            },
            new SortOption
            {
                Value = "Artist",
                FriendlyName = "Artist".GetLocalizedString(),
                FirstSortOption = "SongArtistName",
                SecondSortOption =  "Name"
            },
            new SortOption
            {
                Value = "Album",
                FriendlyName = "Album".GetLocalizedString(),
                FirstSortOption = "Album",
                SecondSortOption =  "Name"
            },
            new SortOption
            {
                Value = "ReleaseYearAscending", 
                FriendlyName = "ReleaseYearAscending".GetLocalizedString(),
                FirstSortOption = "ReleaseYear",
                SecondSortOption = "Name"
            },
            new SortOption
            {
                Value = "ReleaseYearDescending", 
                FriendlyName = "ReleaseYearDescending".GetLocalizedString(),
                FirstSortOption = "ReleaseYear",
                SecondSortOption = "Name"
            },
            new SortOption
            {
                Value = "DateAdded", 
                FriendlyName = "DateAdded".GetLocalizedString(),
                FirstSortOption = "DateAdded",
                SecondSortOption = "Name"
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
                    ApplicationData.Current.LocalSettings.Values["SongSortOption"] = value.Value;
                    Sort(value);
                }
            }
        }

        private void Sort(SortOption value)
        {
            LibrarySongs.Clear();
            if (value.Value == "A-Z" || value.Value == "Artist" || value.Value == "Album")
            {
                var d = from a in App.AppViewModel.LibrarySongs.OrderBy(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(value.FirstSortOption).GetValue(a, null).ToString().ToUpper().First().CheckChar().DeAccent();
                this.SongSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = true,
                    Source = this.SongSource,
                };
                LibrarySongs.AddRange(App.AppViewModel.LibrarySongs.OrderBy(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()));
            }
            else if (value.Value == "ReleaseYearAscending")
            {
                var d = from a in App.AppViewModel.LibrarySongs.OrderBy(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(value.FirstSortOption).GetValue(a, null).ToString();
                this.SongSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = true,
                    Source = this.SongSource,
                };
                LibrarySongs.AddRange(App.AppViewModel.LibrarySongs.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()));
            }
            else if (value.Value == "ReleaseYearDescending")
            {
                var d = from a in App.AppViewModel.LibrarySongs.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(value.FirstSortOption).GetValue(a, null).ToString();
                this.SongSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = true,
                    Source = this.SongSource,
                };
                LibrarySongs.AddRange(App.AppViewModel.LibrarySongs.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()));
            }
            else if (value.Value == "DateAdded")
            {
                var d = from a in App.AppViewModel.LibrarySongs.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()) group a by a.GetType().GetProperty(value.FirstSortOption).GetValue(a, null).ToString();
                this.SongSource = new ObservableGroupedCollection<string, object>(d);
                this.CollectionView = new CollectionViewSource
                {
                    IsSourceGrouped = false,
                    Source = App.AppViewModel.LibrarySongs.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()),
                };
                LibrarySongs.AddRange(App.AppViewModel.LibrarySongs.OrderByDescending(x => x.GetType().GetProperty(value.FirstSortOption).GetValue(x, null).ToString()).ThenBy(x => x.GetType().GetProperty(value.SecondSortOption).GetValue(x, null).ToString()));
            }
        }

        public SongPageViewModel()
        {
            LibrarySongs = new ObservableCollection<LibrarySong>();
            if (ApplicationData.Current.LocalSettings.Values["SongSortOption"] != null)
            {
                try
                {
                    SelectedSortOption = SortValues.Where(x => x.Value == (string)ApplicationData.Current.LocalSettings.Values["SongSortOption"]).FirstOrDefault();
                }
                catch
                {
                    SelectedSortOption = SortValues.First();
                }
            }
            else
            {
                SelectedSortOption = SortValues.First();
            }
        }
        private ObservableCollection<LibrarySong> librarySongs;
        public ObservableCollection<LibrarySong> LibrarySongs
        {
            get
            {
                return librarySongs;
            }
            set
            {
                librarySongs = value;
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

        private ObservableGroupedCollection<string, object> songSource;

        public ObservableGroupedCollection<string, object> SongSource
        {
            get { return songSource; }
            set
            {
                songSource = value;
                NotifyPropertyChanged();
            }
        }
    }
}
