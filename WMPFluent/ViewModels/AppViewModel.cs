using Newtonsoft.Json;
//using WMPFluent.Controls;
using WMPFluent.Interactions;
using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace WMPFluent.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool isMediaAvailable = false;
        public bool IsMediaAvailable
        {
            get 
            { 
                return isMediaAvailable; 
            }
            set 
            { 
                isMediaAvailable = value;
                NotifyPropertyChanged();
            }
        }
        public AppViewModel()
        {
            LibraryPlaylists = new ObservableCollection<LibraryPlaylist>();
            LookForMediaFile(); 
        }

        public async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            // Set the title of the share operation
            e.Request.Data.Properties.Title = "Sharing a picture";

            // Set the description of the share operation
            e.Request.Data.Properties.Description = "Check out this cool picture!";

            // Get the picture file from your app's assets or local storage
            //StorageFile pictureFile = await StorageFile.GetFileFromPathAsync((string)PlayerControls.CurrentPlaybackItem.Source.CustomProperties["AlbumArt"]);

            // Set the picture as the thumbnail for the share operation
            //e.Request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(pictureFile);

            // Add the picture file to the share operation
            //e.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(pictureFile));

            // Add text to the share operation
            //e.Request.Data.SetText($"Now playing {PlayerControls.TrackName} by { PlayerControls.ArtistName} on {Package.Current.DisplayName}");
        }

        public async void LookForMediaFile()
        {
            IsMediaAvailable = await FindMediaFile();
            if (IsMediaAvailable)
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("MusicLibrary.json");
                LibraryAlbums = new ObservableCollection<LibraryAlbum>(JsonConvert.DeserializeObject<ObservableCollection<LibraryAlbum>>(File.ReadAllText(file.Path)).OrderBy(x => x.Name));
                InitiateWatcher();
            }
        }

        private async void InitiateWatcher()
        {
            var musicLibrary = await Windows.Storage.StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
            //var watcher = new LibraryManager.LibraryWatcher(musicLibrary.Folders.Select(x => x.Path).ToArray());
        }

        public void NavigateToHomePage()
        {
            //App.RootNavigationService.Navigate(App.RootNavigationService.RootNavigationPages.First());
        }
        private async Task<bool> FindMediaFile()
        {
            bool mediafound = false;
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("MusicLibrary.json");
                if (file != null)
                {
                    mediafound = true;
                    //return true;
                }
                else
                {
                    mediafound = false;
                }
            }
            catch (FileNotFoundException ex)
            {
                mediafound = false;
            }
            return mediafound;
        }

        //internal void HookMedia(MediaPlayerElement mediaPlayerHost, CustomMediaTransportControls mediaPlayerControls)
        //{
        //    this.MediaPlayerHost = mediaPlayerHost;
        //    this.PlayerControls = mediaPlayerControls;
        //}

        private ObservableCollection<LibraryAlbum> recentAlbums;
        public ObservableCollection<LibraryAlbum> RecentAlbums
        {
            get
            {
                return recentAlbums;
            }
            set
            {
                recentAlbums = value;
                NotifyPropertyChanged();
            }
        }
        private ObservableCollection<LibraryPlaylist> libraryPlaylists;
        public ObservableCollection<LibraryPlaylist> LibraryPlaylists
        {
            get
            {
                return libraryPlaylists;
            }
            set { libraryPlaylists = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<LibrarySong> librarySongs;
        public ObservableCollection<LibrarySong> LibrarySongs
        {
            get
            {
                return librarySongs;
            }
            set { librarySongs = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<LibraryAlbum> libraryAlbums;
        public ObservableCollection<LibraryAlbum> LibraryAlbums
        {
            get { return libraryAlbums; }
            set
            {
                libraryAlbums = value;
                RecentAlbums = new ObservableCollection<LibraryAlbum>(value.OrderByDescending(x => x.DateAdded).Take(12));
                LibrarySongs = new ObservableCollection<LibrarySong>(LibraryAlbums.SelectMany(x => x.Songs));
                NotifyPropertyChanged();
            }
        }

        public MediaPlayerElement MediaPlayerHost { get; private set; }
        //public CustomMediaTransportControls PlayerControls { get; private set; }
        private MediaPlaybackList mediaPlaybackList = null;
        internal DataTransferManager dataTransferManager;

        public MediaPlaybackList MediaPlaybackList
        {
            get
            {
                return mediaPlaybackList;
            }
            set
            {
                if (mediaPlaybackList != value)
                {
                    mediaPlaybackList = value;
                    OnMediaPlayBackListChanged();
                }
            }
        }

        private void OnMediaPlayBackListChanged()
        {
            MediaPlaybackList.Items.VectorChanged += Items_VectorChanged;
            MediaPlaybackList.CurrentItemChanged += MediaPlaybackList_CurrentItemChanged;
        }

        private void Items_VectorChanged(Windows.Foundation.Collections.IObservableVector<MediaPlaybackItem> sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
        {
            
        }
        private ObservableCollection<Models.NowPlayingItem> nowPlayingList;
        public ObservableCollection<Models.NowPlayingItem> NowPlayingList
        {
            get
            {
                return nowPlayingList;
            }
            set
            {
                nowPlayingList = value;
                NotifyPropertyChanged();
            }
        }
        private async void MediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (args.NewItem != null)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    //PlayerControls.CurrentPlaybackItem = args.NewItem;
                    NowPlayingList.ToList().ForEach(x =>
                    {
                        if (x.MediaPlaybackItem == args.NewItem)
                        {
                            x.IsNowPlaying = true;
                        }
                        else
                        {
                            x.IsNowPlaying = false;
                        }
                    });
                    librarySongs.ToList().ForEach(x =>
                    {
                        if (x.Id == (string)mediaPlaybackList.CurrentItem.Source.CustomProperties["Identifier"])
                        {
                            x.IsPlaying = true;
                        }
                        else
                        {
                            x.IsPlaying = false;
                        }
                    });
                    LibraryAlbums.ToList().ForEach(x =>
                    {
                        if (x.Id == (string)mediaPlaybackList.CurrentItem.Source.CustomProperties["AlbumIdentifier"])
                        {
                            x.IsPlaying = true;
                        }
                        else
                        {
                            x.IsPlaying = false;
                        }
                    });
                    Interactions.ControlInteractions.GetAlbumArtColor();
                });
                UpdatePlayerSsession(false);
            }
            else
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //PlayerControls.CurrentPlaybackItem = null;
                    //PlayerControls.ShuffleEnabled = false;
                });
            }
        }

        public async void TryFecthPreviousPlayingSession()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("PlayingListState.json");
                if (file != null)
                {
                    var playingliststore = JsonConvert.DeserializeObject<PlayListStore>(File.ReadAllText(file.Path));
                    MediaInteractions.LoadFromPlaySession(playingliststore);
                }
            }
            catch
            {

            }
        }
        private ObservableCollection<object> breadcrumbNavigationItems;
        public ObservableCollection<object> BreadcrumbNavigationItems
        {
            get
            {
                return breadcrumbNavigationItems;
            }
            set
            {
                breadcrumbNavigationItems = value;
                NotifyPropertyChanged();
            }
        }
        internal void ShuffleToggled(object sender, EventArgs e)
        {
            if ((bool)((AppBarToggleButton)sender).IsChecked)
            {
                MediaPlaybackList.ShuffleEnabled = true;
                var rnd = new Random();
                var shuffledlist = MediaPlaybackList.Items.OrderBy(x => rnd.Next()).ToList();
                if (MediaPlaybackList.CurrentItem != null)
                {
                    shuffledlist.Remove(MediaPlaybackList.CurrentItem);
                    shuffledlist.Insert(0, MediaPlaybackList.CurrentItem);
                }
                MediaPlaybackList.SetShuffledItems(shuffledlist);
                UpdatePlayerSsession();
            }
            else
            {
                MediaPlaybackList.ShuffleEnabled = false;
                UpdatePlayerSsession(true);
            }
        }

        public async void UpdatePlayerSsession(bool updateList = true)
        {
            if (MediaPlaybackList.CurrentItem != null)
            {
                if (updateList)
                {
                    if (App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
                    {

                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            
                            App.AppViewModel.NowPlayingList = new ObservableCollection<Models.NowPlayingItem>(App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList().Select(x => new Models.NowPlayingItem
                            {
                                MediaPlaybackItem = x,
                                Image = (string)x.Source.CustomProperties["AlbumArt"],
                                Title = (string)x.Source.CustomProperties["Title"],
                                Album = (string)x.Source.CustomProperties["Album"],
                                Artist = (string)x.Source.CustomProperties["Artist"],
                                Time = (string)x.Source.CustomProperties["Time"],
                                AlbumIdentifier = (string)x.Source.CustomProperties["AlbumIdentifier"],
                            }));
                            NowPlayingList.ToList().ForEach(x =>
                            {
                                if (x.MediaPlaybackItem == mediaPlaybackList.CurrentItem)
                                {
                                    x.IsNowPlaying = true;
                                }
                                else
                                {
                                    x.IsNowPlaying = false;
                                }
                            });
                        });
                    }
                    else
                    {

                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            App.AppViewModel.NowPlayingList = new ObservableCollection<Models.NowPlayingItem>(App.AppViewModel.MediaPlaybackList.Items.ToList().Select(x => new Models.NowPlayingItem
                            {
                                MediaPlaybackItem = x,
                                Image = (string)x.Source.CustomProperties["AlbumArt"],
                                Title = (string)x.Source.CustomProperties["Title"],
                                Album = (string)x.Source.CustomProperties["Album"],
                                Artist = (string)x.Source.CustomProperties["Artist"],
                                Time = (string)x.Source.CustomProperties["Time"],
                                AlbumIdentifier = (string)x.Source.CustomProperties["AlbumIdentifier"],
                            })); 
                            NowPlayingList.ToList().ForEach(x =>
                            {
                                if (x.MediaPlaybackItem == mediaPlaybackList.CurrentItem)
                                {
                                    x.IsNowPlaying = true;
                                }
                                else
                                {
                                    x.IsNowPlaying = false;
                                }
                            });
                        });
                    }
                }
                var playinglistStore = new Models.PlayListStore
                {
                    CurrentItemIndex = MediaPlaybackList.ShuffleEnabled ? MediaPlaybackList.ShuffledItems.ToList().IndexOf(MediaPlaybackList.CurrentItem) : (int)MediaPlaybackList.CurrentItemIndex,
                    Items = new ObservableCollection<Models.PlayStateTrack>(MediaPlaybackList.Items.Select(x => new Models.PlayStateTrack
                    {
                        Path = (string)x.Source.CustomProperties["Path"],
                        ItemIndex = MediaPlaybackList.Items.IndexOf(x),
                        ShuffledItemIndex = MediaPlaybackList.ShuffleEnabled ? MediaPlaybackList.ShuffledItems.ToList().IndexOf(x) : MediaPlaybackList.Items.IndexOf(x)
                    })),
                    IsShuffleEnabled = MediaPlaybackList.ShuffleEnabled,
                    RepeatEnabled = MediaPlaybackList.AutoRepeatEnabled,
                    ItemOrder = MediaPlaybackList.ShuffleEnabled ? MediaPlaybackList.ShuffledItems.Select(x => MediaPlaybackList.Items.ToList().IndexOf(x)).ToArray() : MediaPlaybackList.Items.Select(x => MediaPlaybackList.Items.ToList().IndexOf(x)).ToArray()
                };
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("PlayingListState.json", CreationCollisionOption.ReplaceExisting);
                File.WriteAllText(file.Path, JsonConvert.SerializeObject(playinglistStore, Formatting.Indented));
            }
        }

        internal void RepeatChanged(object sender, EventArgs e)
        {
            UpdatePlayerSsession(false);
        }

        public MediaPlayer MediaPlayer { get; internal set; }
    }
}
