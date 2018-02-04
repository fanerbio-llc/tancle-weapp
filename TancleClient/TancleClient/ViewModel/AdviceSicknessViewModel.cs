using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Collections.Generic;
using System.Linq;
using TancleClient.ViewModel.Implementation;
using TancleDataModel.Model;

namespace TancleClient.ViewModel
{
    public class AdviceSicknessViewModel : DataList<Advice>
    {
        public override void ResetItems()
        {
            Items?.Clear();

            DataService.LoadAllTuples()?.ToList().ForEach(x =>
            {
                Items.Add(new DataListItem
                {
                    Id = x.Id,
                    Name = x.AdviceName
                });
            });
        }

        public override void SetCheckedItems(List<Advice> items)
        {
            Items?.ForEach(x => x.IsChecked = false);

            items?.ForEach(x =>
            {
                var item = Items?.SingleOrDefault(y => y.Id == x.Id);
                if (item != null)
                {
                    item.IsChecked = true;
                }
            });
        }


    }
}
