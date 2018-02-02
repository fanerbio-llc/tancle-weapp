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
        bool Update<TDbContext, TEntity>(BaseDataAccessService<TDbContext, TEntity> dataService, int entityId, string popUpText, ICopyable<TEntity> copyImpl, bool popUpConfirm)
            where TDbContext : DbContext
            where TEntity : class;
    }
}
