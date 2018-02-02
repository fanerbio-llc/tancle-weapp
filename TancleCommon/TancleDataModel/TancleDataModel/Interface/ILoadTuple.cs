using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TancleDataModel.Interface
{
    public interface ILoadTuple<TEntity> where TEntity : class
    {
        TEntity LoadSingleTuple(int id);

        IEnumerable<TEntity> LoadAllTuples();

        IEnumerable<TEntity> LoadTuples(Expression<Func<TEntity, bool>> whereLambda);

        IEnumerable<TEntity> LoadPageTuples<TS>(
            int pageIndex,
            int pageSize,
            out int total,
            Expression<Func<TEntity, bool>> whereLambda,
            bool isAsc,
            Expression<Func<TEntity, TS>> orderByLambda);

        IEnumerable<TEntity> LoadTuplesWithRelatedTuples<TS1, TS2, TS3, TS4, TS5>(
            Expression<Func<TEntity, bool>> whereLambda,
            Expression<Func<TEntity, TS1>> path1 = null,
            Expression<Func<TEntity, TS2>> path2 = null,
            Expression<Func<TEntity, TS3>> path3 = null,
            Expression<Func<TEntity, TS4>> path4 = null,
            Expression<Func<TEntity, TS5>> path5 = null);

        IEnumerable<TEntity> LoadPageTuplesWithRelatedTuples<TS, TS1, TS2, TS3, TS4, TS5>(
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
            Expression<Func<TEntity, TS5>> path5 = null);
    }
}
