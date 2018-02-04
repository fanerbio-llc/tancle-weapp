using System.Windows.Controls;
using TancleClient.ViewModel;

namespace TancleClient.View
{
    /// <summary>
    /// DataList.xaml 的交互逻辑
    /// </summary>
    public partial class DataList : UserControl
    {
        public DataList()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                DataListItem item = e.AddedItems[0] as DataListItem;
                if (item != null)
                {
                    item.IsChecked = !item.IsChecked;
                }
            }

            // Set to fire SelectionChanged again while check the same item
            list.SelectedIndex = -1;
        }
    }
}
