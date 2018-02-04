using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Model;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelUpdateSickness
    {
        bool AddSickness(Sickness sickness, string popUpText, bool popUpConfirm);

        bool UpdateSickness(Sickness sickness, string popUpText, bool popUpConfirm);
    }
}
