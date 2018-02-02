using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Implementation;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelDeleteItem
    {
        bool Delete<TDbContext, TEntity>(BaseDataAccessService<TDbContext, TEntity> dataService, int entityId, string popUpText, bool popUpConfirm)
            where TDbContext : DbContext
            where TEntity : class;
    }
}
