using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleClient.ViewModel.Interface
{
    public interface IDataList<TEntity> where TEntity: class
    {
        void ResetItems();

        void SetCheckedItems(List<TEntity> items);

        int[] GetCheckedItems();
    }
}
