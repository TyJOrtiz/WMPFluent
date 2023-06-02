using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace WMPFluent.Extensions
{
    public static class ControlExtensions
    {
        public static void SelectAll(this AutoSuggestBox autoSuggestBox) 
        {
            try
            {
                var tb = autoSuggestBox.FindDescendant<TextBox>();
                tb.SelectAll();
            }
            catch
            {

            }
        }
    }
}
