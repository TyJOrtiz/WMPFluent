using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace WMPFluent.Models
{
    public class NavigationObject
    {
        public string Name { get; set; }
        public IconElement Icon { get; set; }
        public bool IsSelectEnabled { get; set; } = true;
        public ObservableCollection<object> Items { get; set; }
    }
}
