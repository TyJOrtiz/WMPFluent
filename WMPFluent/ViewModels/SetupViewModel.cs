using Newtonsoft.Json;
using WMPFluent.Extensions;
using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace WMPFluent.ViewModels
{
    public class SetupViewModel : INotifyPropertyChanged
    {
        public SetupViewModel()
        {
            Albums = new ObservableCollection<LibraryAlbum>();
            StartLookingForMusic();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //KeyValuePair<KeyValuePair<string, string>, LibraryAlbum>
        private ObservableCollection<LoggedItem> loggedAlbums;
        public ObservableCollection<LoggedItem> LoggedAlbums
        {
            get
            {
                return loggedAlbums;
            }
            set
            {
                loggedAlbums = value;
            }
        }
        private async void StartLookingForMusic()
        {
            LoggedAlbums = new ObservableCollection<LoggedItem>();
            var library = await Windows.Storage.StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
            var folders = library.Folders;
            for (int i = 0; i < folders.Count; i++)
            {
                await ProcessFolder(folders[i] as StorageFolder);
            }
            FileRequestedForCollage?.Invoke(Albums, EventArgs.Empty);
            await SerializeJson();
        }

        private async Task SerializeJson()
        {
            var json = JsonConvert.SerializeObject(Albums.Where(x => x.Songs.Count > 0).OrderBy(x => x.Id).Distinct(), Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("MusicLibrary.json", CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(file, json);

            //App.ApplicationViewModel.LoadPlaylists();
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                IsSetupComplete = true;
            });
        }
        private bool isSetupComplete = false;
        public bool IsSetupComplete
        {
            get
            {
                return isSetupComplete;
            }
            set
            {
                if (isSetupComplete != value)
                {
                    isSetupComplete = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private async Task ProcessFolder(StorageFolder storageFolder)
        {
            for (int i = 0; i < 1000000;)
            {
                var items = await storageFolder.GetItemsAsync((uint)i, 30);
                if (items.Any())
                {
                    for (int j = 0; j < items.Count;j++)
                    {
                        var processed = await ProcessItem(items[j]);
                        if (processed)
                        {
                            try
                            {
                                //Debug.WriteLine(String.Format("item {0} processed, moving to {1}", items[j].Name, items[j + 1].Name));

                            }
                            catch
                            {

                            }
                        }
                    }
                    i = i + items.Count();
                    //Debug.WriteLine(i);
                }
                else
                {
                    break;
                }
            }
        }
        private ObservableCollection<LibraryAlbum> albums;
        public ObservableCollection<LibraryAlbum> Albums
        {
            get
            {
                return albums;
            }
            set
            {
                albums = value;
            }
        }
        private int _filesProcessed = 0;
        public int FilesProcessed
        {
            get { return _filesProcessed; }
            set { _filesProcessed = value; NotifyPropertyChanged(); }
        }
        public static event EventHandler FileRequestedForCollage;
        public List<string> SearchedArtists = new List<string>();
        private async Task<bool> ProcessItem(IStorageItem item)
        {
            bool proccessed = false;
            if (item.IsOfType(StorageItemTypes.File) && (item as StorageFile).IsFileSupported())
            { 
                var file = (StorageFile)item;
                var song = await file.GenerateSong(false);
                var albums12 = LoggedAlbums.ToList().Where(x => x.Key == song.AlbumArtist && x.Value == song.Album);
                if (albums12.Any())
                {
                    //Debug.WriteLine("hey theres an album here");
                    var albums1 = albums12.FirstOrDefault();
                    albums1.Album.Songs.Add(song);
                    song.AlbumIdentifier = albums1.Album.Id;
                    albums1.Album.Songs = new ObservableCollection<LibrarySong>(albums1.Album.Songs.OrderBy(x => x.Track));
                    song.AlbumArt = albums1.Album.AlbumArt;
                    proccessed = true;
                }
                else
                {
                    var album = new LibraryAlbum
                    {
                        Name = song.Album,
                        ArtistName = song.AlbumArtist,
                        Genre = song.Genre,
                        Year = song.ReleaseYear,
                        DateAdded = song.DateAdded,
                        Id = (song.Album + " " + song.AlbumArtist).EncodeTo64(),
                    };
                    if (!SearchedArtists.Contains(album.ArtistName))
                    {
                        //lets see how many times we can do this before we get locked out lol
                        var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ArtistBackgroundImages", CreationCollisionOption.OpenIfExists);
                        try
                        {
                            var img = await folder.GetFileAsync($"{album.ArtistName.MakeSafeForFileName()}.jpg");
                            if (img != null)
                            {
                                SearchedArtists.Add(album.ArtistName);
                            }
                        }
                        catch
                        {
                            //var artistImage = await Helpers.WebHelpers.GetImageForArtist(album.ArtistName);
                            //if (!string.IsNullOrEmpty(artistImage))
                            //{
                            //    try
                            //    {
                            //        var client = new WebClient();
                            //        byte[] imageBytes = client.DownloadData(artistImage);
                            //        var img = await folder.CreateFileAsync($"{album.ArtistName.MakeSafeForFileName()}.jpg", CreationCollisionOption.ReplaceExisting);
                            //        // Save the image to a file
                            //        File.WriteAllBytes(img.Path, imageBytes);
                            //    }
                            //    catch
                            //    {

                            //    }
                            //}
                            SearchedArtists.Add(album.ArtistName);
                        }
                    }
                    album.Songs = new ObservableCollection<LibrarySong>
                    {
                        song
                    };
                    album.AlbumArt = await file.GetAlbumArt();
                    song.AlbumArt = album.AlbumArt;
                    song.AlbumIdentifier = album.Id;
                    Albums.Add(album);
                    LoggedAlbums.Add(new Models.LoggedItem
                    {
                        Key = song.AlbumArtist, 
                        Value = song.Album,
                        Album = album
                    });
                    proccessed = true;
                    FilesProcessed += 1;
                    ////Debug.WriteLine(loggedAlbums.Last());
                }
            }
            else if(item.IsOfType(StorageItemTypes.Folder))
            {
                await ProcessFolder(item as StorageFolder);
                proccessed = true;
            }
            return proccessed;
        }
    }
}
