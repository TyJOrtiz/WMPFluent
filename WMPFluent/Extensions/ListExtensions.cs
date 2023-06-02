using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WMPFluent.Extensions
{
    public static class ListExtensions
    {
        public static async void Serialize<T>(this IEnumerable<T> ts, string fileName)
        {
            var json = JsonConvert.SerializeObject(ts, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(file, json);
        }
        public static async void Serialize<T>(this IList<T> ts, string fileName)
        {
            var json = JsonConvert.SerializeObject(ts, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(file, json);
        }
        public static void ForEach<T>(this ObservableCollection<T> ts, Action<T> action)
        {
            foreach (var item in ts)
            {
                action(item);
            }
        }
        public static void AddRange<T>(this Collection<T> ts, IEnumerable<T> list)
        {
            list.ToList().ForEach(x => ts.Add(x));
        }
        public static void AddRange<T>(this Collection<T> ts, IList<T> list)
        {
            list.ToList().ForEach(x => ts.Add(x));
        }
        public static async void Serialize<T>(this ObservableCollection<T> ts, string fileName)
        {
            var json = JsonConvert.SerializeObject(ts, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(file, json);
        }
    }
}
