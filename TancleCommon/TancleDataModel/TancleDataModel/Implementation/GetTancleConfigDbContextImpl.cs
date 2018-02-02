using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Interface;

namespace TancleDataModel.Implementation
{
    public class GetTancleConfigDbContextImpl : IGetDbContext<TancleConfigDbContext>
    {
        /// <summary>
        /// Require unique DbContext object in the same thread
        /// Solve: 一个实体对象不能由多个 IEntityChangeTracker 实例引用
        /// </summary>
        /// <returns></returns>
        public TancleConfigDbContext GetDbContextInstance()
        {
            TancleConfigDbContext dbContext = CallContext.GetData("DbContext") as TancleConfigDbContext;
            if(dbContext == null)
            {
                dbContext = new TancleConfigDbContext();
                CallContext.SetData("DbContext", dbContext);
            }
            return dbContext;
        }
    }
}
