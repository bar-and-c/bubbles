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
        // Add/remove DataTemplate properties as applicable. 
        public DataTemplate OtherTemplate { get; set; }
        public DataTemplate BubbleTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is GameObject)
            {
                // Later on, there might be more GameObject classes than Bubble, something like Obstacle, and possibly other...
                //GameObject gameObject = (GameObject)item;
                DataTemplate dt = item is Bubble ? this.BubbleTemplate : this.OtherTemplate;
                return dt;
            }
            return base.SelectTemplate(item, container);
        }

    }

}
