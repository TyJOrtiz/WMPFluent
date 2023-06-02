using Prism.Commands;
using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.Storage;
using Microsoft.Toolkit.Uwp;
using Windows.System;
using Windows.ApplicationModel.DataTransfer;
using System.Diagnostics;
//using WMPFluent.ContentPages;
using WMPFluent.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Newtonsoft.Json;
//using WMPFluent.Controls;
using Windows.UI.Xaml.Controls;
using WMPFluent.Extensions;

namespace WMPFluent.Interactions
{
    public class MediaInteractions
    {
        public static DelegateCommand PlayAll(ObservableCollection<LibrarySong> songs)
        {
            return new DelegateCommand(new Action(delegate () { Load(songs, false); }));
        }
        public static DelegateCommand GoToAlbumPage(string id)
        {
            return new DelegateCommand(new Action(delegate () { GetalbumandNavigate(id); }));
        }

        public static DelegateCommand PlayAll(LibraryAlbum album)
        {
            return new DelegateCommand(new Action(delegate () { Load(album.Songs, false); }));
        }
        public static DelegateCommand ShuffleAll(ObservableCollection<LibrarySong> songs)
        {
            return new DelegateCommand(new Action(delegate () { Load(songs, true); }));
        }
        public static DelegateCommand ShuffleAll(ObservableCollection<LibraryAlbum> albums)
        {
            return new DelegateCommand(new Action(delegate () { Load(new ObservableCollection<LibrarySong>(albums.SelectMany(x => x.Songs)), true); }));
        }
        public static DelegateCommand PlayAll(ObservableCollection<LibraryAlbum> albums)
        {
            return new DelegateCommand(new Action(delegate () { Load(new ObservableCollection<LibrarySong>(albums.SelectMany(x => x.Songs)), false); }));
        }
        public static DelegateCommand PlaySingle(LibrarySong song)
        {
            return new DelegateCommand(new Action(delegate () { Load(new ObservableCollection<LibrarySong> { song }, false); }));
        }
        public static DelegateCommand NavigateToArtist(string artist)
        {
            return new DelegateCommand(new Action(delegate ()
            {
                var selectedArtistViewModel = new SelectedArtistPageViewModel(artist);
                //App.RootNavigationService.NavigateToNewPage(new SelectedArtistPage(), selectedArtistViewModel);
            }));
        }
        public static DelegateCommand AddSingleToPlayNext(LibrarySong song)
        {
            return new DelegateCommand(new Action(delegate () { AddToPlayingList(new ObservableCollection<LibrarySong> { song }, AddToPlaylistMode.AfterCurrentTrack); }));
        }
        public static DelegateCommand AddSingleToPlayLater(LibrarySong song)
        {
            return new DelegateCommand(new Action(delegate () { AddToPlayingList(new ObservableCollection<LibrarySong> { song }, AddToPlaylistMode.Last); }));
        }
        public static DelegateCommand AddSongsToPlayNext(ObservableCollection<LibrarySong> songs)
        {
            return new DelegateCommand(new Action(delegate () { AddToPlayingList(songs, AddToPlaylistMode.AfterCurrentTrack); }));
        }
        public static DelegateCommand AddSongsToPlayLater(ObservableCollection<LibrarySong> songs)
        {
            return new DelegateCommand(new Action(delegate () { AddToPlayingList(songs, AddToPlaylistMode.Last); }));
        }
        public static DelegateCommand LocateFileOnDisk(string path)
        {
            return new DelegateCommand(new Action(delegate () { FindFile(path); }));
        }
        public static DelegateCommand ShareCurrentSong()
        {
            return new DelegateCommand(new Action(delegate ()
            {
                DataTransferManager.ShowShareUI();
            }));
        }
        public static DelegateCommand AddOrRemoveTag(LibraryAlbum libraryAlbum, bool hasTag, string id)
        {
            return new DelegateCommand(new Action(delegate ()
            {
                AddOrRemoveAlbumTag(libraryAlbum, hasTag, id);
            }));
        }
        public static DelegateCommand DeleteTag(string id)
        {
            return new DelegateCommand(new Action(async delegate ()
            {
                var dialog = new ContentDialog
                {
                    Title = "Delete tag",
                    Content = "Are you sure you want to delete this tag?",
                    PrimaryButtonText = "Delete",
                    SecondaryButtonText = "Cancel"
                };
                var result = await dialog.ShowAsync();
                switch (result)
                {
                    case ContentDialogResult.Primary:
                        try
                        {
                            //App.RootNavigationService.Tags.Remove(App.RootNavigationService.Tags.FirstOrDefault(x => x.Id == id));
                            //App.RootNavigationService.RootNavigationPages.Last().Children.Remove(App.RootNavigationService.RootNavigationPages.Last().Children.FirstOrDefault(x => x.GetType() == typeof(Tag) && ((Tag)x).Id == id));
                            //App.AppViewModel.LibraryAlbums.Where(x => x.TagIds != null && x.TagIds.Contains(id)).ToList().ForEach(x =>
                            //{
                            //    try
                            //    {
                            //        x.TagIds.Remove(id);
                            //    }
                            //    catch
                            //    {

                            //    }
                            //});
                            //var json = JsonConvert.SerializeObject(App.RootNavigationService.Tags, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                            //{
                            //    NullValueHandling = NullValueHandling.Ignore,
                            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            //});
                            //var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Tags.json", CreationCollisionOption.OpenIfExists);
                            //await Windows.Storage.FileIO.WriteTextAsync(file, json);

                            //await SerializeJson();
                        }
                        catch
                        {

                        }
                        break;
                    case ContentDialogResult.Secondary:
                        break;
                    default:
                        break;
                }
            }));
        }
        private static async void AddOrRemoveAlbumTag(LibraryAlbum libraryAlbum, bool hasTag, string id)
        {
            //if (libraryAlbum.TagIds == null)
            //{
            //    libraryAlbum.TagIds = new ObservableCollection<string>();
            //}
            //if (hasTag)
            //{
            //    libraryAlbum.TagIds.Remove(id);
            //    App.RootNavigationService.Tags.FirstOrDefault(x => x.Id == id).NotifyTagRemoved(libraryAlbum);
            //}
            //else
            //{
            //    libraryAlbum.TagIds.Add(id);
            //    App.RootNavigationService.Tags.FirstOrDefault(x => x.Id == id).NotifyTagAdded(libraryAlbum);
            //}
            //await SerializeJson();

        }
        public static DelegateCommand AddNewTag(LibraryAlbum libraryAlbum)
        {
            return new DelegateCommand(new Action(delegate ()
            {
                AddNewAlbumTag(libraryAlbum);
            }));
        }

        private static async void AddNewAlbumTag(LibraryAlbum libraryAlbum)
        {

            //var tagdialog = new TagDialog();
            //tagdialog.DataContext = libraryAlbum;
            //await tagdialog.ShowAsync();
        }
        public static DelegateCommand AddNewTag()
        {
            return new DelegateCommand(new Action(delegate ()
            {
                AddNewAlbumTag();
            }));
        }

        private static async void AddNewAlbumTag()
        {

            //var tagdialog = new TagDialog();
            //await tagdialog.ShowAsync();
        }
        public static async Task SerializeJson()
        {
            var json = JsonConvert.SerializeObject(App.AppViewModel.LibraryAlbums.OrderBy(x => x.Id).Distinct(), Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("MusicLibrary.json", CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(file, json);
        }
        private static void GetalbumandNavigate(string id)
        {
            var album = App.AppViewModel.LibraryAlbums.Where(x => x.Id == id).FirstOrDefault();
            if (album != null)
            {
                var selectedAlbumViewModel = new SelectedAlbumViewModel(album);
                //App.RootNavigationService.NavigateToNewPage(new SelectedAlbumPage(), selectedAlbumViewModel, true);
            }
        }

        private static async void FindFile(string path)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            var containingFolder = await file.GetParentAsync();
            var launchOptions = new FolderLauncherOptions();
            launchOptions.ItemsToSelect.Add(file);
            await Launcher.LaunchFolderAsync(containingFolder, launchOptions);
        }

        private static async void Load(ObservableCollection<LibrarySong> songs, bool shuffle)
        {
            if (App.AppViewModel.MediaPlaybackList != null)
            {
                App.AppViewModel.MediaPlayer.Source = null;
            }
            App.AppViewModel.MediaPlaybackList = new MediaPlaybackList();
            for (int i = 0; i < songs.Count; i++)
            {
                LibrarySong song = songs[i];
                var binder = new MediaBinder();
                binder.Token = song.Path;
                binder.Binding += Binder_Binding;
                var mediasource = Windows.Media.Core.MediaSource.CreateFromMediaBinder(binder);
                var playbackitem = new MediaPlaybackItem(mediasource);
                playbackitem.AutoLoadedDisplayProperties = AutoLoadedDisplayPropertyKind.Music;
                var properties = playbackitem.GetDisplayProperties();
                properties.Type = MediaPlaybackType.Music;
                mediasource.CustomProperties.Add("AlbumArt", song.AlbumArt ?? "ms-appx:///Assets/Placeholder-light.png");
                //mediasource.CustomProperties.Add("LoadedOrder", song.PlayIndex);
                mediasource.CustomProperties.Add("Album", song.Album);
                mediasource.CustomProperties.Add("AlbumIdentifier", song.AlbumIdentifier);
                mediasource.CustomProperties.Add("Identifier", song.Id);
                mediasource.CustomProperties.Add("Title", song.Name);
                mediasource.CustomProperties.Add("Artist", song.SongArtistName);
                mediasource.CustomProperties.Add("Time", song.Duration);
                mediasource.CustomProperties.Add("Path", song.Path);
                mediasource.CustomProperties.Add("AlbumArtist", song.AlbumArtist);

                if (song.AlbumArt == "ms-appx:///Assets/Placeholder.png" || song.AlbumArt == "ms-appx:///Assets/Placeholder-light.png" || song.AlbumArt == "ms-appx:///Assets/Placeholder-dark.png")
                {
                    StorageFile imagefile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(song.AlbumArt));
                    var stream = await imagefile.OpenAsync(FileAccessMode.Read);
                    properties.Thumbnail = RandomAccessStreamReference.CreateFromStream(stream);
                }
                else
                {
                    try
                    {
                        StorageFile imagefile = await StorageFile.GetFileFromPathAsync(song.AlbumArt);
                        var stream = await imagefile.OpenAsync(FileAccessMode.Read);
                        properties.Thumbnail = RandomAccessStreamReference.CreateFromStream(stream);
                    }
                    catch
                    {
                        StorageFile imagefile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Placeholder-light.png"));
                        var stream = await imagefile.OpenAsync(FileAccessMode.Read);
                        properties.Thumbnail = RandomAccessStreamReference.CreateFromStream(stream);
                    }
                }
                playbackitem.ApplyDisplayProperties(properties);
                App.AppViewModel.MediaPlaybackList.Items.Add(playbackitem);
            };
            App.AppViewModel.MediaPlayer = new MediaPlayer
            {
                AutoPlay = true,
            };
            if (ApplicationData.Current.LocalSettings.Values["AppVolume"] != null)
            {
                App.AppViewModel.MediaPlayer.Volume = (double)ApplicationData.Current.LocalSettings.Values["AppVolume"];
            }
            App.AppViewModel.MediaPlayer.VolumeChanged += (v, c) =>
            {
                ApplicationData.Current.LocalSettings.Values["AppVolume"] = v.Volume;
            };
            if (shuffle == true)
            {
                //App.AppViewModel.PlayerControls.ShuffleEnabled = true;
                App.AppViewModel.MediaPlaybackList.ShuffleEnabled = shuffle;
            }
            else
            {

                //App.AppViewModel.PlayerControls.ShuffleEnabled = false;
            }
            if (App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
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
            }
            else
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
            }
            App.AppViewModel.MediaPlayer.Source = App.AppViewModel.MediaPlaybackList;
            App.AppViewModel.MediaPlayerHost.SetMediaPlayer(App.AppViewModel.MediaPlayer);
            App.AppViewModel.MediaPlayer.Play();
        }
        private static async void AddToPlayingList(ObservableCollection<LibrarySong> songs, AddToPlaylistMode addToPlaylistMode)
        {
            if (App.AppViewModel.MediaPlaybackList != null && App.AppViewModel.MediaPlayer.Source != null)
            {
                int f = App.AppViewModel.MediaPlaybackList.ShuffleEnabled ? (int)App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList().IndexOf(App.AppViewModel.MediaPlaybackList.CurrentItem) + 1 : (int)App.AppViewModel.MediaPlaybackList.CurrentItemIndex + 1;
                for (int i = 0; i < songs.Count; i++)
                {
                    LibrarySong song = songs[i];
                    var binder = new MediaBinder();
                    binder.Token = song.Path;
                    binder.Binding += Binder_Binding;
                    var mediasource = Windows.Media.Core.MediaSource.CreateFromMediaBinder(binder);
                    var playbackitem = new MediaPlaybackItem(mediasource);
                    playbackitem.AutoLoadedDisplayProperties = AutoLoadedDisplayPropertyKind.Music;
                    var properties = playbackitem.GetDisplayProperties();
                    properties.Type = MediaPlaybackType.Music;
                    mediasource.CustomProperties.Add("AlbumArt", song.AlbumArt);
                    //mediasource.CustomProperties.Add("LoadedOrder", song.PlayIndex);
                    mediasource.CustomProperties.Add("Album", song.Album);
                    mediasource.CustomProperties.Add("AlbumIdentifier", song.AlbumIdentifier);
                    mediasource.CustomProperties.Add("Identifier", song.Id);
                    mediasource.CustomProperties.Add("Title", song.Name);
                    mediasource.CustomProperties.Add("Artist", song.SongArtistName);
                    mediasource.CustomProperties.Add("Time", song.Duration);
                    mediasource.CustomProperties.Add("Path", song.Path);
                    mediasource.CustomProperties.Add("AlbumArtist", song.AlbumArtist);

                    if (song.AlbumArt == "ms-appx:///Assets/Placeholder.png")
                    {
                        StorageFile imagefile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(song.AlbumArt));
                        var stream = await imagefile.OpenAsync(FileAccessMode.Read);
                        properties.Thumbnail = RandomAccessStreamReference.CreateFromStream(stream);
                    }
                    else
                    {
                        StorageFile imagefile = await StorageFile.GetFileFromPathAsync(song.AlbumArt);
                        var stream = await imagefile.OpenAsync(FileAccessMode.Read);
                        properties.Thumbnail = RandomAccessStreamReference.CreateFromStream(stream);
                    }
                    playbackitem.ApplyDisplayProperties(properties);
                    if (addToPlaylistMode == AddToPlaylistMode.Last)
                    {
                        if (!App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
                        {
                            App.AppViewModel.MediaPlaybackList.Items.Add(playbackitem);
                        }
                        else
                        {
                            App.AppViewModel.MediaPlaybackList.Items.Add(playbackitem);
                            var items = App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList();
                            items.Remove(playbackitem);
                            items.Add(playbackitem);
                            App.AppViewModel.MediaPlaybackList.SetShuffledItems(items);
                        }
                    }
                    else
                    {
                        if (!App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
                        {
                            App.AppViewModel.MediaPlaybackList.Items.Insert(f, playbackitem);
                        }
                        else
                        {
                            App.AppViewModel.MediaPlaybackList.Items.Insert(f, playbackitem);
                            //Debug.WriteLine(App.AppViewModel.MediaPlaybackList.Items.Count);
                            var items = App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList();
                            items.Remove(playbackitem);
                            items.Insert(f, playbackitem);
                            App.AppViewModel.MediaPlaybackList.SetShuffledItems(items);
                        }
                        f++;
                    }

                };
                App.AppViewModel.UpdatePlayerSsession();
            }
            else
            {
                Load(songs, false);
            }
        }
        private static async void Binder_Binding(MediaBinder sender, MediaBindingEventArgs args)
        {
            try
            {
                var deferral = args.GetDeferral();
                var file = await StorageFile.GetFileFromPathAsync(args.MediaBinder.Token);
                args.SetStorageFile(file);
                deferral.Complete();
            }
            catch
            {

            }
        }

        internal static async void LoadFromPlaySession(PlayListStore playingliststore)
        {
            var songs = new ObservableCollection<LibrarySong>();
            try
            {
                foreach (var p in playingliststore.Items)
                {
                    var song = App.AppViewModel.LibrarySongs.Where(x => x.Path == p.Path).FirstOrDefault();
                    if (song != null)
                    {
                        songs.Add(song);
                    }
                    else
                    {
                        playingliststore.Items.Remove(p);
                    }
                }
                App.AppViewModel.MediaPlaybackList = new MediaPlaybackList();
                for (int i = 0; i < songs.Count; i++)
                {
                    LibrarySong song = songs[i];
                    var binder = new MediaBinder();
                    binder.Token = song.Path;
                    binder.Binding += Binder_Binding;
                    var mediasource = Windows.Media.Core.MediaSource.CreateFromMediaBinder(binder);
                    var playbackitem = new MediaPlaybackItem(mediasource);
                    playbackitem.AutoLoadedDisplayProperties = AutoLoadedDisplayPropertyKind.Music;
                    var properties = playbackitem.GetDisplayProperties();
                    properties.Type = MediaPlaybackType.Music;
                    mediasource.CustomProperties.Add("AlbumArt", song.AlbumArt);
                    //mediasource.CustomProperties.Add("LoadedOrder", song.PlayIndex);
                    mediasource.CustomProperties.Add("Album", song.Album);
                    mediasource.CustomProperties.Add("AlbumIdentifier", song.AlbumIdentifier);
                    mediasource.CustomProperties.Add("Identifier", song.Id);
                    mediasource.CustomProperties.Add("Title", song.Name);
                    mediasource.CustomProperties.Add("Artist", song.SongArtistName);
                    mediasource.CustomProperties.Add("Time", song.Duration);
                    mediasource.CustomProperties.Add("Path", song.Path);
                    mediasource.CustomProperties.Add("ItemOrder", playingliststore.Items[i].ItemIndex);
                    mediasource.CustomProperties.Add("ShuffledItemOrder", playingliststore.Items[i].ShuffledItemIndex);
                    mediasource.CustomProperties.Add("AlbumArtist", song.AlbumArtist);

                    if (song.AlbumArt == "ms-appx:///Assets/Placeholder.png")
                    {
                        StorageFile imagefile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(song.AlbumArt));
                        var stream = await imagefile.OpenAsync(FileAccessMode.Read);
                        properties.Thumbnail = RandomAccessStreamReference.CreateFromStream(stream);
                    }
                    else
                    {
                        StorageFile imagefile = await StorageFile.GetFileFromPathAsync(song.AlbumArt);
                        var stream = await imagefile.OpenAsync(FileAccessMode.Read);
                        properties.Thumbnail = RandomAccessStreamReference.CreateFromStream(stream);
                    }
                    playbackitem.ApplyDisplayProperties(properties);
                    App.AppViewModel.MediaPlaybackList.Items.Add(playbackitem);
                };
                App.AppViewModel.MediaPlayer = new MediaPlayer
                {
                    AutoPlay = false,
                };
                if (ApplicationData.Current.LocalSettings.Values["AppVolume"] != null)
                {
                    App.AppViewModel.MediaPlayer.Volume = (double)ApplicationData.Current.LocalSettings.Values["AppVolume"];
                }
                App.AppViewModel.MediaPlayer.VolumeChanged += (v, c) =>
                {
                    ApplicationData.Current.LocalSettings.Values["AppVolume"] = v.Volume;
                };
                App.AppViewModel.MediaPlaybackList.AutoRepeatEnabled = playingliststore.RepeatEnabled;
                if (playingliststore.IsShuffleEnabled == true)
                {
                    //App.AppViewModel.PlayerControls.ShuffleEnabled = true;
                    App.AppViewModel.MediaPlaybackList.ShuffleEnabled = true;
                    //App.AppViewModel.MediaPlaybackList.SetShuffledItems(App.AppViewModel.MediaPlaybackList.Items.OrderBy(i => playingliststore.ItemOrder[App.AppViewModel.MediaPlaybackList.Items.IndexOf(i)]));
                    App.AppViewModel.MediaPlaybackList.SetShuffledItems(App.AppViewModel.MediaPlaybackList.Items.OrderBy(i => i.Source.CustomProperties["ShuffledItemOrder"]));
                    var list = new List<MediaPlaybackItem>();
                    foreach (var item in playingliststore.ItemOrder)
                    {
                        var d = App.AppViewModel.MediaPlaybackList.Items[item];
                        list.Add(d);
                    }
                    //App.AppViewModel.MediaPlaybackList.SetShuffledItems(list);
                }
                else
                {
                    //App.AppViewModel.PlayerControls.ShuffleEnabled = false;
                }
                if (App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
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
                }
                else
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
                }
                //App.AppViewModel.MediaPlaybackList.StartingItem = !App.AppViewModel.PlayerControls.ShuffleEnabled ? App.AppViewModel.MediaPlaybackList.Items[playingliststore.CurrentItemIndex] : App.AppViewModel.MediaPlaybackList.Items[playingliststore.ItemOrder[playingliststore.CurrentItemIndex]];
                App.AppViewModel.MediaPlayer.Source = App.AppViewModel.MediaPlaybackList;
                App.AppViewModel.MediaPlayerHost.SetMediaPlayer(App.AppViewModel.MediaPlayer);
            }
            catch
            {

            }
        }
        public static DelegateCommand MoveUp(NowPlayingItem item)
        {
            return new DelegateCommand(new Action(delegate ()
            {
                MoveItemUp(item);
            }));
        }
        public static DelegateCommand MoveDown(NowPlayingItem item)
        {
            return new DelegateCommand(new Action(delegate ()
            {
                MoveItemDown(item);
            }));
        }
        public static DelegateCommand MoveNext(NowPlayingItem item)
        {
            return new DelegateCommand(new Action(delegate ()
            {
                MoveToNext(item);
            }));
        }
        public static DelegateCommand RemoveItem(NowPlayingItem item)
        {
            return new DelegateCommand(new Action(delegate ()
            {
                Remove(item);
            }));
        }

        public static DelegateCommand ClearPlayingList()
        {
            return new DelegateCommand(new Action(delegate ()
            {
                ClearList();
            }));
        }
        public static DelegateCommand AddObjectToPlaylist(dynamic objectToAdd, LibraryPlaylist playlist)
        {
            return new DelegateCommand(new Action(async () =>
            {
                int objectsAdded = 0;
                var file = await StorageFile.GetFileFromPathAsync(playlist.Path);
                string count = "";
                if (System.IO.Path.GetExtension(file.Path).Equals(".wpl", StringComparison.InvariantCultureIgnoreCase) || System.IO.Path.GetExtension(file.Path).Equals(".zpl", StringComparison.InvariantCultureIgnoreCase))
                {
                    var playlistfile = await Windows.Media.Playlists.Playlist.LoadAsync(file);
                    if (objectToAdd is LibrarySong song)
                    {
                        var songfile = await StorageFile.GetFileFromPathAsync(song.Path);
                        playlistfile.Files.Add(songfile);
                        objectsAdded++;
                    }
                    else if (objectToAdd is LibraryAlbum album)
                    {
                        foreach (var albumSong in album.Songs)
                        {
                            var songfile = await StorageFile.GetFileFromPathAsync(albumSong.Path);
                            playlistfile.Files.Add(songfile);
                            objectsAdded++;
                        }
                    }
                    await playlistfile.SaveAsync();
                    count = objectsAdded > 1 ? "songs" : "song";
                }
                else if (System.IO.Path.GetExtension(file.Path).Contains(".m3u", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (objectToAdd is LibrarySong song)
                    {
                        await FileIO.AppendLinesAsync(file, new[] { song.Path });
                        objectsAdded++;
                    }
                    else if (objectToAdd is LibraryAlbum album)
                    {
                        await FileIO.AppendLinesAsync(file, album.Songs.Select(x => x.Path));
                        objectsAdded += album.Songs.Count;
                    }
                    count = objectsAdded > 1 ? "songs" : "song";
                }
                //Debug.WriteLine($"{objectsAdded} {count} added to '{playlist.Name}'");
            }));
        }
        private static async void ClearList()
        {
            App.AppViewModel.MediaPlaybackList.Items.Clear();
            App.AppViewModel.MediaPlayer.Source = null;
            App.AppViewModel.NowPlayingList.Clear();
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync("PlayingListState.json");
            if (file != null)
            {
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
        }

        internal static void MoveItemUp(NowPlayingItem item)
        {
            var index = App.AppViewModel.NowPlayingList.IndexOf(item);
            if (App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
            {
                var list = App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList();
                list.Remove(item.MediaPlaybackItem);
                list.Insert(index - 1, item.MediaPlaybackItem);
                App.AppViewModel.MediaPlaybackList.SetShuffledItems(list);
                App.AppViewModel.NowPlayingList.Remove(item);
                App.AppViewModel.NowPlayingList.Insert(index - 1, item);
                App.AppViewModel.UpdatePlayerSsession(false);
            }
            else
            {
                App.AppViewModel.MediaPlaybackList.Items.Remove(item.MediaPlaybackItem);
                App.AppViewModel.MediaPlaybackList.Items.Insert(index - 1, item.MediaPlaybackItem);
                App.AppViewModel.NowPlayingList.Remove(item);
                App.AppViewModel.NowPlayingList.Insert(index - 1, item);
                App.AppViewModel.UpdatePlayerSsession(false);
            }
        }
        internal static void MoveItemDown(NowPlayingItem item)
        {
            var index = App.AppViewModel.NowPlayingList.IndexOf(item);
            if (App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
            {
                var list = App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList();
                list.Remove(item.MediaPlaybackItem);
                list.Insert(index + 1, item.MediaPlaybackItem);
                App.AppViewModel.MediaPlaybackList.SetShuffledItems(list);
                App.AppViewModel.NowPlayingList.Remove(item);
                App.AppViewModel.NowPlayingList.Insert(index + 1, item);
                App.AppViewModel.UpdatePlayerSsession(false);
            }
            else
            {
                App.AppViewModel.MediaPlaybackList.Items.Remove(item.MediaPlaybackItem);
                App.AppViewModel.MediaPlaybackList.Items.Insert(index + 1, item.MediaPlaybackItem);
                App.AppViewModel.NowPlayingList.Remove(item);
                App.AppViewModel.NowPlayingList.Insert(index + 1, item);
                App.AppViewModel.UpdatePlayerSsession(false);
            }
        }

        internal static void MoveToNext(NowPlayingItem item)
        {
            if (App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
            {
                var list = App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList();
                list.Remove(item.MediaPlaybackItem);
                list.Insert(list.IndexOf(App.AppViewModel.MediaPlaybackList.CurrentItem) + 1, item.MediaPlaybackItem);
                App.AppViewModel.MediaPlaybackList.SetShuffledItems(list);
                App.AppViewModel.NowPlayingList.Remove(item);
                App.AppViewModel.NowPlayingList.Insert(App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList().IndexOf(item.MediaPlaybackItem), item);
                App.AppViewModel.UpdatePlayerSsession(false);
            }
            else
            {
                App.AppViewModel.MediaPlaybackList.Items.Remove(item.MediaPlaybackItem);
                App.AppViewModel.MediaPlaybackList.Items.Insert((int)(App.AppViewModel.MediaPlaybackList.CurrentItemIndex + 1), item.MediaPlaybackItem);
                App.AppViewModel.NowPlayingList.Remove(item);
                App.AppViewModel.NowPlayingList.Insert((int)(App.AppViewModel.MediaPlaybackList.CurrentItemIndex + 1), item);
                App.AppViewModel.UpdatePlayerSsession(false);
            }
        }

        internal static void Remove(NowPlayingItem item)
        {
            App.AppViewModel.NowPlayingList.Remove(item);
            App.AppViewModel.MediaPlaybackList.Items.Remove(item.MediaPlaybackItem);
            try
            {
                var list = App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList();
                try
                {
                    list.Remove(item.MediaPlaybackItem);
                }
                catch
                {

                }
                App.AppViewModel.MediaPlaybackList.SetShuffledItems(list);
            }
            catch
            {

            }
            App.AppViewModel.UpdatePlayerSsession(false);
        }

        internal static void MovePlayingItemInList(NowPlayingItem item, int v)
        {
            TimeSpan x;
            bool isItemCurrent = false;
            if (App.AppViewModel.MediaPlaybackList.CurrentItem == item.MediaPlaybackItem)
            {
                x = App.AppViewModel.MediaPlayer.PlaybackSession.Position;
                isItemCurrent = true;
            }
            if (App.AppViewModel.MediaPlaybackList.ShuffleEnabled)
            {
                var list = App.AppViewModel.MediaPlaybackList.ShuffledItems.ToList();
                list.Remove(item.MediaPlaybackItem);
                list.Insert(v, item.MediaPlaybackItem);
                App.AppViewModel.MediaPlaybackList.SetShuffledItems(list);
                App.AppViewModel.UpdatePlayerSsession(false);
            }
            else
            {
                App.AppViewModel.MediaPlaybackList.Items.Remove(item.MediaPlaybackItem);
                App.AppViewModel.MediaPlaybackList.Items.Insert(v, item.MediaPlaybackItem);
                if (isItemCurrent)
                {
                    App.AppViewModel.MediaPlaybackList.MoveTo((uint)App.AppViewModel.MediaPlaybackList.Items.IndexOf(item.MediaPlaybackItem));
                    App.AppViewModel.MediaPlayer.PlaybackSession.Position = x;
                }
                App.AppViewModel.UpdatePlayerSsession(false);
            }
        }


        internal static async void LoadFileList(List<StorageFile> storageFiles)
        {
            var songs = new ObservableCollection<LibrarySong>();
            foreach (var x in storageFiles) 
            {
                var s = await x.GenerateSong(true, true);
                songs.Add(s);
            };
            await Task.Delay(200);
            Load(songs, false);
        }
        private enum AddToPlaylistMode
        {
            AfterCurrentTrack,
            Last
        }
    }
}
