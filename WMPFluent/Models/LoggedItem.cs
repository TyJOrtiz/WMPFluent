using System.Collections.Generic;

namespace WMPFluent.Models
{
    public class LoggedItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public LibraryAlbum Album { get; set; }
    }
}