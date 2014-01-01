using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bubbles
{
    public class GameObjectTemplateSelector : DataTemplateSelector
    {
        // TODO: Change these names! Add/remove DataTemplate properties as applicable. 
        public DataTemplate CustTemplate { get; set; }
        public DataTemplate BubbleTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is GameObject)
            {
                // TODO: Later on, there will be a GameObject class with subclasses Bubble, something like Obstacle, and possibly other
                Bubble gameObject = (Bubble)item;
                DataTemplate dt = gameObject is Bubble ? this.BubbleTemplate : this.CustTemplate;
                return dt;
            }
            return base.SelectTemplate(item, container);
        }

    }

}
