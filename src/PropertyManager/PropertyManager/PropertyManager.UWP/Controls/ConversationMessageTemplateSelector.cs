using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PropertyManager.Models;

namespace PropertyManager.UWP.Controls
{
    public class ConversationMessageTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var conversation = item as ConversationModel;
            if (conversation == null)
            {
                return base.SelectTemplateCore(item, container);
            }

            var resources = Application.Current.Resources;
            if (conversation.UniqueSenders.Any(s => s.Contains("Simon")))
            {
                return resources["ConversationMessageRightTemplate"] as DataTemplate;
            }
            return resources["ConversationMessageLeftTemplate"] as DataTemplate;
        }
    }
}
