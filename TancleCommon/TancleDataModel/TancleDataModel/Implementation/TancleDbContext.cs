using BaseCommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Interface;

namespace TancleDataModel.Implementation
{
    public class TancleDbContext<TDbContext> where TDbContext: DbContext
    {
        private static IGetDbContext<TDbContext> _getDbContext;

        public TancleDbContext(IGetDbContext<TDbContext> getDbContextImpl)
        {
            _getDbContext = getDbContextImpl;
        }

        public TDbContext DbContext
        {
            get
            {
                try
                {
                    return _getDbContext.GetDbContextInstance();
                }
                catch (Exception e)
                {
                    LogHelper.Log.Error("_getDbContext is not set yet", e);
                    return null;
                }
            }
        }
    }
}
