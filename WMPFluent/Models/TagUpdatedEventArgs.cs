using System;

namespace WMPFluent.Models
{
    internal class TagUpdatedEventArgs : EventArgs
    {
        public object ObjectAdded { get; set; }
        public object ObjectRemoved { get; set; }
        public TagUpdateReason Reason { get; set; }
    }
    public enum TagUpdateReason
    {
        ItemAdded,
        ItemRemoved,
    }
}