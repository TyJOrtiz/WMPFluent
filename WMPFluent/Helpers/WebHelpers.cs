using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WMPFluent.Helpers
{
    public class WebHelpers
    {
        internal static async Task<string> GetImageForArtist(string artist)
        {
            string path = null;
            string artistName = artist;
            string apiKey = "a09838379e68f06adeb3ceedbd2524f6";
            string queryUrl = $"https://musicbrainz.org/ws/2/artist/?query={Uri.EscapeDataString(artistName)}&limit=1&fmt=json";

            // Make the API request to search for the artist
            try
            {
                HttpClient musicbrainzclient = new HttpClient();
                musicbrainzclient.DefaultRequestHeaders.Add("User-Agent", $"WMPFluent/3.0.0.0 ( tyler.j.ortiz@outlook.com )");
                var d = await musicbrainzclient.GetAsync(queryUrl); 
                WebClient client = new WebClient();
                string responseString = await d.Content.ReadAsStringAsync();

                // Extract the artist's MBID from the response
                JObject responseJson = JObject.Parse(responseString);
                string artistMbid = responseJson["artists"][0]["id"].ToString();

                // Make the API request to the Cover Art Archive to get the artist's image
                string imageUrl = $"https://webservice.fanart.tv/v3/music/{artistMbid}?api_key={apiKey}&includes=artistthumb";
                byte[] imageBytes = client.DownloadData(imageUrl);
                string oneBigString = Encoding.ASCII.GetString(imageBytes);
                var responseJson2 = JObject.Parse(oneBigString);
                List<string> uris = new List<string>();
                for (int i = 0; ; i++)
                {
                    try
                    {
                        uris.Add(responseJson2["artistthumb"][i]["url"].ToString());
                    }
                    catch
                    {
                        break;
                    }
                }
                Random rnd = new Random();
                int index = rnd.Next(uris.Count);
                //string imageUrl = $"https://coverartarchive.org/release-group/{artistMbid}/front";
                //byte[] imageBytes = client.DownloadData(imageUrl);

                // Save the image to a file
                //File.WriteAllBytes(img.Path, backgroundImageUrl);
                path = uris[index];
            }
            catch
            {
                return null;
            }
            return path;
        }
    }
}
