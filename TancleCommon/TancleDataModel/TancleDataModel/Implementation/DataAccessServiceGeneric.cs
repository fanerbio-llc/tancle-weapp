using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Implementation;
using TancleDataModel.Interface;

namespace TancleDataModel.Implementation
{
    public class DataAccessServiceGeneric<TDbContext, TEntity> : BaseDataAccessService<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        public DataAccessServiceGeneric(IGetDbContext<TDbContext> getDbContextImpl): base(getDbContextImpl)
        {
            SetBehaviorOfLoadTuple(new DataAccessGenericImpl<TDbContext, TEntity>(getDbContextImpl));
            SetBehaviorOfAddTuple(new DataAccessGenericImpl<TDbContext, TEntity>(getDbContextImpl));
            SetBehaviorOfModifyTuple(new DataAccessGenericImpl<TDbContext, TEntity>(getDbContextImpl));
            SetBehaviorOfDeleteTuple(new DataAccessGenericImpl<TDbContext, TEntity>(getDbContextImpl));
        }
    }
}
