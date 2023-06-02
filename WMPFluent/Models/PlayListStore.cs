using System.Collections.ObjectModel;

namespace WMPFluent.Models
{
    public class PlayListStore
    {
        public int CurrentItemIndex { get; set; }
        public ObservableCollection<PlayStateTrack> Items { get; set; }
        public bool IsShuffleEnabled { get; set; }
        public bool RepeatEnabled { get; set; }
        public int[] ItemOrder { get; set; }
    }
}