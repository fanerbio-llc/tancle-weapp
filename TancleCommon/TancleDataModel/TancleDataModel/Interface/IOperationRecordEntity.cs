using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleDataModel.Interface
{
    public interface IOperationRecordEntity
    {
        string RecordActionName();

        object RecordEntity(DbContext context);
    }
}
