using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelConfirmWindow
    {
        bool ConfirmWindow(INotifyPropertyChanged vm, string confirmText);
    }
}
