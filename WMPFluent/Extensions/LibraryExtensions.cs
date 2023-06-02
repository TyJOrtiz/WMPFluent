using WMPFluent.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WMPFluent.Extensions
{
    public static class LibraryExtensions
    {
        public static bool HasFeature(this LibrarySong s, string artist)
        {
            List<string> featwords = new List<string>
            {
                "feat. ",
                "with ",
                "featuring ",
            };
            string featlist = "";
            try
            {
                if (s.Name.Contains(artist, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var f in featwords)
                    {
                        try
                        {
                            featlist = s.Name.ToLower().Substring(s.Name.ToLower().IndexOf(f) - 1);
                        }
                        catch 
                        {
                            //break;
                        }
                    }
                }
            }
            catch
            {
                //return false;
            }
            return (!string.IsNullOrEmpty(featlist) && featlist.Contains(artist, StringComparison.OrdinalIgnoreCase));
            //return (s.Title.ContainsAny(new[] { $"feat. {artist}", $"featuring {artist}", $"(with {artist})", $"and {artist}", $"& {artist}" }, StringComparison.OrdinalIgnoreCase));
        }
    }
}
