using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMPFluent.Models
{
    public class AppNotifcationArgs : EventArgs
    {
        public object Image { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public string Icon { get; set; }
        public bool Actionable { get; set; }
        public NotificationType NotificationType { get; set; }
    }
    public enum NotificationType
    {
        NowPlaying,
        AddedToQueue,
        AddedToPlaylist
    }
}
