using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleClient.ViewModel;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelUpdateDisplayList
    {
        void UpdateDataList(int pageIndex, out int total, int itemPerPage = 10);
    }
}
