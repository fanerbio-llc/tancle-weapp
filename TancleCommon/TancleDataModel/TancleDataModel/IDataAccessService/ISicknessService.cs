using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Implementation;
using TancleDataModel.Model;

namespace TancleDataModel.IDataAccessService
{
    public interface ISicknessService
    {
        DataAccessResult ModifySickness(Sickness sickness, int[] habitList, int[] adviceList, int[] areaList);
    }
}
