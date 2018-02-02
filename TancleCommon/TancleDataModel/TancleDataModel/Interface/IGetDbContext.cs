using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleDataModel.Interface
{
    public interface IGetDbContext<out TDbContext> where TDbContext: DbContext
    {
        TDbContext GetDbContextInstance();
    }
}
