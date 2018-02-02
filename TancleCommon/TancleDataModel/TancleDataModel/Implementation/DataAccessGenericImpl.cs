using BaseCommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Interface;

namespace TancleDataModel.Implementation
{
    public class DataAccessGenericImpl<TDbContext, TEntity> : TancleDbContext<DbContext>,
            ILoadTuple<TEntity>,
            IAddTuple<TEntity>,
            IModifyTuple<TEntity>,
            IDeleteTuple<TEntity>
        where TDbContext: DbContext
        where TEntity: class
    {
        public DataAccessGenericImpl(IGetDbContext<TDbContext> getDbContextImpl): base(getDbContextImpl)
        {
        }

        public IEnumerable<TEntity> LoadAllTuples()
        {
            try
            {
                return DbContext.Set<TEntity>().ToList();
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("LoadAllTuples() exception.", dbEx);
                return null;
            }
        }

        public TEntity LoadSingleTuple(int id)
        {
            try
            {
                return DbContext.Set<TEntity>().Find(id);
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("LoadSingleTuple(int id) exception.", dbEx);
                return null;
            }
        }

        public IEnumerable<TEntity> LoadTuples(Expression<Func<TEntity, bool>> whereLambda)
        {
            try
            {
                return DbContext.Set<TEntity>().Where(whereLambda);
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("LoadSingleTuple(int id) exception.", dbEx);
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
            if (pageIndex <= 0)
            {
                total = 0;
                return null;
            }

            try
            {
                var temp = DbContext.Set<TEntity>().Where(whereLambda);

                // Get total item amount
                total = temp.Count();

                // Sort
                if (isAsc)
                {
                    temp = temp.OrderBy(orderByLambda);
                }
                else
                {
                    temp = temp.OrderByDescending(orderByLambda);
                }
                
                temp = temp.Skip(pageSize * (pageIndex - 1))    // Skip specfied quantity of data
                           .Take(pageSize);                     // Take specfied quantity of data

                return temp.ToList(); // eager loading
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("LoadPageTuples() exception.", dbEx);
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
                return
                    path5 != null
                    ? DbContext.Set<TEntity>()
                        .Include(path1)
                        .Include(path2)
                        .Include(path3)
                        .Include(path4)
                        .Include(path5)
                        .Where(whereLambda).ToList()
                        : (
                            path4 != null
                            ? DbContext.Set<TEntity>()
                                .Include(path1)
                                .Include(path2)
                                .Include(path3)
                                .Include(path4)
                                .Where(whereLambda).ToList()
                                : (
                                    path3 != null
                                    ? DbContext.Set<TEntity>()
                                        .Include(path1)
                                        .Include(path2)
                                        .Include(path3)
                                        .Where(whereLambda).ToList()
                                        : (
                                            path2 != null
                                            ? DbContext.Set<TEntity>()
                                                .Include(path1)
                                                .Include(path2)
                                                .Where(whereLambda).ToList()
                                                : (
                                                    path1 != null
                                                    ? DbContext.Set<TEntity>()
                                                        .Include(path1)
                                                        .Where(whereLambda).ToList()
                                                        : DbContext.Set<TEntity>().Where(whereLambda).ToList()))));
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("LoadEntitiesWithRelatedEntities() exception.", dbEx);
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
            if (pageIndex <= 0)
            {
                total = 0;
                return null;
            }

            try
            {
                var temp = LoadTuplesWithRelatedTuples(whereLambda, path1, path2, path3, path4, path5);

                // Get total item amount
                total = temp.Count();

                // Sort
                if (isAsc)
                {
                    temp = temp.OrderBy(orderByLambda.Compile());
                }
                else
                {
                    temp = temp.OrderByDescending(orderByLambda.Compile());
                }

                temp = temp.Skip(pageSize * (pageIndex - 1))    // Skip specfied quantity of data
                           .Take(pageSize);                     // Take specfied quantity of data

                return temp.ToList(); // eager loading
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("LoadPageEntities() exception.", dbEx);
                total = 0;
                return null;
            }
        }

        public DataAccessResult Add(TEntity entity)
        {
            var result = new DataAccessResult();

            try
            {
                DbContext.Set<TEntity>().Add(entity);

                if (DbContext.SaveChanges() > 0)
                {
                    result.ResultCode = ResultCodeOption.Ok;
                    result.Message = DataAccessResult.SuccessDefaultString;
                }
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("Add(TEntity entity) exception.", dbEx);
                result.Message = dbEx.GetType().ToString();
            }

            return result;
        }

        public DataAccessResult Modify(TEntity entity)
        {
            var result = new DataAccessResult();

            try
            {
                DbContext.Set<TEntity>().Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;

                if (DbContext.SaveChanges() > 0)
                {
                    result.ResultCode = ResultCodeOption.Ok;
                    result.Message = DataAccessResult.SuccessDefaultString;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                LogHelper.Log.Error("Modify(TEntity entity) exception.", dbEx);
                result.ResultCode = ResultCodeOption.ValidationFailure;
                result.Message = dbEx.InnerException?.InnerException?.Message;
            }
            catch (DbUpdateException dbEx)
            {
                LogHelper.Log.Error("Modify(TEntity entity) exception.", dbEx);
                result.ResultCode = ResultCodeOption.Duplicate;
                result.Message = dbEx.InnerException?.InnerException?.Message;
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("Modify(TEntity entity) exception.", dbEx);
                result.Message = dbEx.GetType().ToString();
            }

            return result;
        }

        public DataAccessResult Delete(int id)
        {
            var result = new DataAccessResult();

            TEntity entity;

            try
            {
                entity = DbContext.Set<TEntity>().Find(id);
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("LoadSingleTuple(int id) exception.", dbEx);
                entity = null;
            }

            if (entity == null)
            {
                result.ResultCode = ResultCodeOption.Ok;
                result.Message = "The specified tuple is not exist.";
                return result;
            }

            try
            {
                DbContext.Set<TEntity>().Attach(entity);
                DbContext.Set<TEntity>().Remove(entity);

                if (DbContext.SaveChanges() > 0)
                {
                    result.ResultCode = ResultCodeOption.Ok;
                    result.Message = DataAccessResult.SuccessDefaultString;
                }
            }
            catch (Exception dbEx)
            {
                result.ResultCode = ResultCodeOption.Failure;
                result.Message = dbEx.GetType().ToString();
            }

            return result;
        }

        public DataAccessResult Delete(TEntity entity)
        {
            var result = new DataAccessResult();

            DbContext.Set<TEntity>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
            //DbContext.Set<TEntity>().Remove(entity);

            try
            {
                if (DbContext.SaveChanges() > 0)
                {
                    result.ResultCode = ResultCodeOption.Ok;
                    result.Message = DataAccessResult.SuccessDefaultString;
                }

            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error("Delete(TEntity entity) exception.", dbEx);
                result.Message = dbEx.GetType().ToString();
            }

            return result;
        }
    }
}
