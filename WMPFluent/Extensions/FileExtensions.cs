using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace WMPFluent.Extensions
{
    public static class FileExtensions
    {
        private static List<string> SupportedFileList = new List<string>
        {
            ".m4a",
            ".mp3",
            ".aac",
            ".wma",
            ".flac",
            ".ogg"
        };

        public static bool IsFileSupported(this StorageFile file)
        {
            if (Path.GetExtension(file.Path).IsOneOf(SupportedFileList))
                return true;
            else
                return false;
        }
        public static async Task<LibrarySong> GenerateSong(this StorageFile file, bool needToCreateAccessToken = true, bool isTempFile = false)
        {
            StorageFile raw = null;
            var song = new LibrarySong();
            try
            {
                if (needToCreateAccessToken)
                {
                    if (Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Entries.Count == 1000)
                    {
                        Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Clear();
                    }
                    string faToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);

                    await Task.Delay(200);

                    raw = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFileAsync(faToken);
                }
                else
                {
                    raw = file;
                }
                var props = await raw.Properties.GetMusicPropertiesAsync();
                string[] contributingArtistsKey = { "System.Music.Artist" };
                IDictionary<string, object> contributingArtistsProperty =
                    await props.RetrievePropertiesAsync(contributingArtistsKey);
                string[] contributingArtists = contributingArtistsProperty["System.Music.Artist"] as string[];
                var artist = "";
                if (contributingArtists == null)
                {
                    artist = String.IsNullOrEmpty(props.AlbumArtist) ? "Unknown artist" : props.AlbumArtist;
                }
                else
                {
                    artist = String.Join("; ", contributingArtists);
                }
                var albumname = String.IsNullOrEmpty(props.Album) ? "Unknown album" : props.Album;
                var aartist = String.IsNullOrEmpty(props.AlbumArtist) ? artist : props.AlbumArtist;
                var year = String.IsNullOrEmpty(props.Year.ToString()) ? "0" : props.Year.ToString();
                var title = String.IsNullOrEmpty(props.Title) ? Path.GetFileNameWithoutExtension(raw.Path) : props.Title;
                var track = String.IsNullOrEmpty(props.TrackNumber.ToString()) ? 0 : (int)props.TrackNumber;
                var id = Guid.NewGuid();
                song.Name = title;
                song.DateAdded = raw.DateCreated.DateTime.ToString(DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern);
                song.AlbumArtist = aartist;
                song.Genre = String.IsNullOrEmpty(props.Genre.FirstOrDefault()) ? "Unknown Genre" : props.Genre.FirstOrDefault();
                song.SongArtistName = artist;
                song.Album = albumname;
                song.Id = raw.FolderRelativeId.Split("\\")[0];
                song.Track = track;
                song.LastPlayedTime = null;
                song.PlayCount = 0;
                song.IsExplicit = false;
                song.Liked = false;
                song.ReleaseYear = year;
                song.AlbumArt = isTempFile ? await GetTempFileAlbumArt(raw) : null;
                song.Path = raw.Path;
                song.Duration = raw.GetTime(props.Duration);

            }
            catch
            {

            }
            return song;
        }

        private static async Task<string> GetTempFileAlbumArt(StorageFile file)
        {
            string path;
            try
            {
                using (var imgThumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 600, ThumbnailOptions.None))
                {
                    if (imgThumbnail != null)
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imgThumbnail.CloneStream());

                        SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();



                        var albumimage = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid().ToString() + ".png", CreationCollisionOption.GenerateUniqueName);

                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.PngEncoderId, await albumimage.OpenAsync(FileAccessMode.ReadWrite));

                        encoder.SetSoftwareBitmap(softwareBitmap);

                        await encoder.FlushAsync();
                        path = albumimage.Path;
                        //////////Debug.WriteLine(path);
                        return path;
                    }
                    else
                    {
                        path = "ms-appx:///Assets/Placeholder.png";
                    }
                }
            }
            catch
            {
                return (path = null);
            }
            return path;
        }

        public static string GetTime(this StorageFile file, TimeSpan duration)
        {
            //////////Debug.WriteLine(duration.ToString(@"hh\:mm\:ss"));
            return $"{duration.Days:#0:;;\\}{duration.Hours:#0:;;\\}{duration.Minutes:00:}{duration.Seconds:00}";
        }
        public static string GetTime(TimeSpan duration)
        {
            //////////Debug.WriteLine(duration.ToString(@"hh\:mm\:ss"));
            return $"{duration.Days:#0:;;\\}{duration.Hours:#0:;;\\}{duration.Minutes:00:}{duration.Seconds:00}";
        }
        public static async Task<string> GetAlbumArt(this StorageFile folderItem)
        {
            string path;
            try
            {
                using (var imgThumbnail = await folderItem.GetThumbnailAsync(ThumbnailMode.MusicView, 600, ThumbnailOptions.None))
                {
                    if (imgThumbnail != null)
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imgThumbnail.CloneStream());

                        SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();



                        var albumimagefolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("AlbumImages", CreationCollisionOption.OpenIfExists);
                        StorageFile file_Save = await albumimagefolder.CreateFileAsync(Guid.NewGuid().ToString() + ".png", CreationCollisionOption.ReplaceExisting);

                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.PngEncoderId, await file_Save.OpenAsync(FileAccessMode.ReadWrite));

                        encoder.SetSoftwareBitmap(softwareBitmap);

                        await encoder.FlushAsync();
                        path = file_Save.Path;
                        //////////Debug.WriteLine(path);
                        return path;
                    }
                    else
                    {
                        path = "ms-appx:///Assets/Placeholder.png";
                    }
                }
            }
            catch
            {
                return (path = null);
            }
            return path;
        }
    }
}
