using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Implementation;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelAddItem
    {
        bool Add<TDbContext, TEntity>(BaseDataAccessService<TDbContext, TEntity> dataService, TEntity entity, string popUpText, bool popUpConfirm)
            where TDbContext : DbContext
            where TEntity : class;
    }
}
