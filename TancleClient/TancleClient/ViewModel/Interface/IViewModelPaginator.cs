using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelPaginator
    {
        void ResetDisplayPage();

        void UpdateDataList(IViewModelUpdateDisplayList updateDisplayListImpl);
    }
}
