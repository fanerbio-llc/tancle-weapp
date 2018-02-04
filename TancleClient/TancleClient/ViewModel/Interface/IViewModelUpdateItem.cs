using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Implementation;
using TancleDataModel.Interface;

namespace TancleClient.ViewModel.Interface
{
    public interface IViewModelUpdateItem
    {
        bool Update<TDbContext, TEntity>(BaseDataAccessService<TDbContext, TEntity> dataService, int entityId, ICopyable<TEntity> copyImpl, string popUpText, bool popUpConfirm)
            where TDbContext : DbContext
            where TEntity : class;
    }
}
