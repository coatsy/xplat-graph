using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace PropertyManager.UWP.Controls
{
    public class AlternateRowListView : ListView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var index = ItemContainerGenerator.IndexFromContainer(element);
            var listViewItem = element as ListViewItem;
            if (listViewItem == null)
            {
                return;
            }

            if (index % 2 == 0)
            {
                listViewItem.Background =
                    Application.Current.Resources["AlternateRowListViewItemBackgroundColorBrush"] 
                    as SolidColorBrush;
            }
        }
    }
}
