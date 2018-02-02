using BaseCommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Interface;

namespace TancleDataModel.Implementation
{
    public class BaseDataAccessService<TDbContext, TEntity> : TancleDbContext<TDbContext>
        where TDbContext: DbContext
        where TEntity: class
    {
        #region Private fields

        private ILoadTuple<TEntity> _loadTuple;
        private IAddTuple<TEntity> _addTuple;
        private IModifyTuple<TEntity> _modifyTuple;
        private IDeleteTuple<TEntity> _deleteTuple;
        #endregion

        #region Constructors

        public BaseDataAccessService(IGetDbContext<TDbContext> getDbContextImpl) : base(getDbContextImpl)
        {
        }
        #endregion

        #region Setter

        public void SetBehaviorOfLoadTuple(ILoadTuple<TEntity> behavior)
        {
            _loadTuple = behavior;
        }

        public void SetBehaviorOfAddTuple(IAddTuple<TEntity> behavior)
        {
            _addTuple = behavior;
        }

        public void SetBehaviorOfModifyTuple(IModifyTuple<TEntity> behavior)
        {
            _modifyTuple = behavior;
        }

        public void SetBehaviorOfDeleteTuple(IDeleteTuple<TEntity> behavior)
        {
            _deleteTuple = behavior;
        }
        #endregion

        #region Getter

        public ILoadTuple<TEntity> GetBehaviorOfLoadTuple()
        {
            return _loadTuple;
        }

        public IAddTuple<TEntity> GetBehaviorOfAddTuple()
        {
            return _addTuple;
        }

        public IModifyTuple<TEntity> GetBehaviorOfModifyTuple()
        {
            return _modifyTuple;
        }

        public IDeleteTuple<TEntity> GetBehaviorOfDeleteTuple()
        {
            return _deleteTuple;
        }
        #endregion

        #region Public functions

        public TEntity LoadSingleTuple(int id)
        {
            try
            {
                return _loadTuple.LoadSingleTuple(id);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_loadTuple is not set yet", e);
                return null;
            }
        }

        public IEnumerable<TEntity> LoadAllTuples()
        {
            try
            {
                return _loadTuple.LoadAllTuples();
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_loadTuple is not set yet", e);
                return null;
            }
        }

        IEnumerable<TEntity> LoadTuples(Expression<Func<TEntity, bool>> whereLambda)
        {
            try
            {
                return _loadTuple.LoadTuples(whereLambda);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_loadTuple is not set yet", e);
                return null;
            }
        }

        public IEnumerable<TEntity> LoadPageTuples<TS>(
            int pageIndex,
            int pageSize,
            out int total,
            Expression<Func<TEntity, bool>> whereLambda,
            bool isAsc,
            Expression<Func<TEntity, TS>> orderByLambda)
        {
            try
            {
                return _loadTuple.LoadPageTuples(pageIndex, pageSize, out total, whereLambda, isAsc, orderByLambda);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_loadTuple is not set yet", e);
                total = 0;
                return null;
            }
        }

        public IEnumerable<TEntity> LoadTuplesWithRelatedTuples<TS1, TS2, TS3, TS4, TS5>(
            Expression<Func<TEntity, bool>> whereLambda,
            Expression<Func<TEntity, TS1>> path1 = null,
            Expression<Func<TEntity, TS2>> path2 = null,
            Expression<Func<TEntity, TS3>> path3 = null,
            Expression<Func<TEntity, TS4>> path4 = null,
            Expression<Func<TEntity, TS5>> path5 = null)
        {
            try
            {
                return _loadTuple.LoadTuplesWithRelatedTuples(whereLambda, path1, path2, path3, path4, path5);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_loadTuple is not set yet", e);
                return null;
            }
        }

        public IEnumerable<TEntity> LoadPageTuplesWithRelatedTuples<TS, TS1, TS2, TS3, TS4, TS5>(
            int pageIndex,
            int pageSize,
            out int total,
            Expression<Func<TEntity, bool>> whereLambda,
            bool isAsc,
            Expression<Func<TEntity, TS>> orderByLambda,
            Expression<Func<TEntity, TS1>> path1 = null,
            Expression<Func<TEntity, TS2>> path2 = null,
            Expression<Func<TEntity, TS3>> path3 = null,
            Expression<Func<TEntity, TS4>> path4 = null,
            Expression<Func<TEntity, TS5>> path5 = null)
        {
            try
            {
                return _loadTuple.LoadPageTuplesWithRelatedTuples(
                    pageIndex, pageSize, out total, whereLambda, isAsc, orderByLambda,
                    path1, path2, path3, path4, path5);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_loadTuple is not set yet", e);
                total = 0;
                return null;
            }
        }

        public DataAccessResult Add(TEntity entity)
        {
            try
            {
                return _addTuple.Add(entity);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_addTuple is not set yet", e);
                return null;
            }
        }

        public DataAccessResult Modify(TEntity entity)
        {
            try
            {
                return _modifyTuple.Modify(entity);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_modifyTuple is not set yet", e);
                return null;
            }
        }

        public DataAccessResult Delete(int id)
        {
            try
            {
                return _deleteTuple.Delete(id);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_deleteTuple is not set yet", e);
                return null;
            }
        }

        public DataAccessResult Delete(TEntity entity)
        {
            try
            {
                return _deleteTuple.Delete(entity);
            }
            catch (Exception e)
            {
                LogHelper.Log.Error("_deleteTuple is not set yet", e);
                return null;
            }
        }
        #endregion
    }
}
