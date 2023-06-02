using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
//using WMPFluent.ContentPages;
using WMPFluent.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace WMPFluent.Models
{
    public class Tag : NavigationObject, INotifyPropertyChanged
    {
        public event EventHandler ItemUpdatedToTag;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void NotifyTagAdded(object objectAdded)
        {
            ItemUpdatedToTag?.Invoke(this, new Models.TagUpdatedEventArgs
            {
                ObjectAdded = objectAdded,
                Reason = TagUpdateReason.ItemAdded
            });
        }
        internal void NotifyTagRemoved(object objectRemoved)
        {
            ItemUpdatedToTag?.Invoke(this, new Models.TagUpdatedEventArgs
            {
                ObjectRemoved = objectRemoved,
                Reason = TagUpdateReason.ItemRemoved
            });
        }

        public Tag() 
        {
            //this.Page = new TagPage();
        }
        [JsonIgnore]
        private Color _uiColor;
        [JsonIgnore]
        public Color UiColor
        {
            get { return _uiColor; }
            set
            {
                if (_uiColor != value)
                {
                    _uiColor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string _id;
        public string Id
        {
            get { return _id; }
            set 
            { 
                _id = value;
                NotifyPropertyChanged();
            }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                NotifyPropertyChanged();
            }
        }
        private string _color;
        public string Color
        {
            get { return _color; }
            set 
            { 
                if (_color != value)
                {
                    _color = value;
                    UiColor = value.ToColor();
                    NotifyPropertyChanged();

                }
            }
        }


    }
}
