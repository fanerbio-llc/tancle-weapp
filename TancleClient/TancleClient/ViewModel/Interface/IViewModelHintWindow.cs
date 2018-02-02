using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelHintWindow
    {
        void HintWindow(INotifyPropertyChanged vm, string hintText);

    }
}
