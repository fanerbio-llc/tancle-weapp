using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelErrorWindow
    {
        void ErrorWindow(INotifyPropertyChanged vm, string errorText);
    }
}
