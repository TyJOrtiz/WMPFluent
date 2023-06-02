using WMPFluent.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Storage.Search;
using Windows.Storage;
using System.Linq;
//using WMPFluent.ContentPages;

namespace WMPFluent.Helpers
{
    public class ContentHelpers
    {
        public static async Task<ObservableCollection<PlaylistObject>> GetPlaylists()
        {
            var Playlists = new ObservableCollection<PlaylistObject>();
            try
            {
                var plfolder = await KnownFolders.MusicLibrary.CreateFolderAsync("Playlists", CreationCollisionOption.OpenIfExists);
                if (plfolder != null)
                {
                    QueryOptions queryOptions = new QueryOptions();
                    queryOptions.FolderDepth = FolderDepth.Deep;
                    queryOptions.FileTypeFilter.Add(".wpl");
                    queryOptions.FileTypeFilter.Add(".zpl");
                    queryOptions.FileTypeFilter.Add(".m3u");
                    queryOptions.FileTypeFilter.Add(".m3u8");
                    StorageFileQueryResult queryResult = plfolder.CreateFileQueryWithOptions(queryOptions);
                    queryResult.ContentsChanged += QueryResult_ContentsChanged;
                    var files = await queryResult.GetFilesAsync();
                    if (files.Any())
                    {
                        //foreach (var file1 in files.ToList().OrderBy(x => x.DisplayName))
                        //{
                        //    var file = await StorageFile.GetFileFromPathAsync(file1.Path);
                        //    var pl = new PlaylistObject(file.Path)
                        //    {
                        //        Label = file.DisplayName,
                        //        Page = new SelectedPlaylistPage(),
                        //    };
                        //    Playlists.Add(pl);
                        //    App.AppViewModel.LibraryPlaylists.Add(new LibraryPlaylist(file.Path)
                        //    {
                        //        Name = file.DisplayName,
                        //        PlaylistObject = pl
                        //    });
                        //}
                    }
                }
                //var plfile = await plfolder.GetFileAsync("Playlists.json");
                //var json = await FileIO.ReadTextAsync(plfile);
                //this.Playlists = new ObservableCollection<Playlist>(JsonConvert.DeserializeObject<ObservableCollection<Playlist>>(json).OrderBy(x => x.Title));
            }
            catch
            {

            }
            return Playlists;
        }

        private static void QueryResult_ContentsChanged(IStorageQueryResultBase sender, object args)
        {

        }
    }
}