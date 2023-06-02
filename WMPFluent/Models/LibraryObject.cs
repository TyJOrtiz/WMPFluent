using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WMPFluent.Models
{
    public class LibraryObject
    {
        [JsonIgnore]
        public string ObjectName { get; set; }
        [JsonIgnore]
        public string Type { get; set; }

        [JsonIgnore]
        public string Category { get; set; }

        [JsonIgnore]
        public bool CanNavigate { get; set; }

    }
}
