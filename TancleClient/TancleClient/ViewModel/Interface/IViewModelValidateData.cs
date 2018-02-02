using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Interface;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelValidateData
    {
        bool Validate(IDataValidatable validator);
    }
}
