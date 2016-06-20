using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using PropertyManager.Models;

namespace PropertyManager.UWP.Controls
{
    //class MessageListView : GridView
    //{
    //    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    //    {
    //        base.PrepareContainerForItemOverride(element, item);
    //        var index = ItemContainerGenerator.IndexFromContainer(element);
    //        var listViewItem = element as GridViewItem;
    //        var d = VisualTreeHelper.GetChildrenCount(element);
    //        var grid = VisualTreeHelper.GetChild(listViewItem, 0);
    //        GridView d; d.ItemTemplateSelector
    //    }

    //    protected override void OnApplyTemplate()
    //    {
    //        base.OnApplyTemplate();
    //    }

    //    protected override void OnItemTemplateChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
    //    {
    //        base.OnItemTemplateChanged(oldItemTemplate, newItemTemplate);
    //    }
    //}

    public class ConversationMessageTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var conversation = item as ConversationModel;
            if (conversation != null)
            {
                var resources = Application.Current.Resources;
                if (conversation.UniqueSenders.Any(s => s.Contains("Simon")))
                {
                    return resources["ConversationMessageRightTemplate"] as DataTemplate;
                }
                return resources["ConversationMessageLeftTemplate"] as DataTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
