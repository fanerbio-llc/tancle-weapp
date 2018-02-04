using BaseCommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.IDataAccessService;
using TancleDataModel.Implementation;
using TancleDataModel.Model;

namespace TancleDataModel.DataAccessService
{
    public class SicknessService : DataAccessServiceGeneric<TancleConfigDbContext, Sickness>, ISicknessService
    {
        public SicknessService(): base(new GetTancleConfigDbContextImpl())
        {
        }

        public DataAccessResult ModifySickness(Sickness sickness, int[] habitList, int[] adviceList, int[] areaList)
        {
            var result = new DataAccessResult();

            var filterHabitList = from x in DbContext.Habits where habitList.Any(y => y == x.Id) select x;
            // Clear exsiting habits
            sickness.Habits = new List<Habit>();
            // Add new habits
            filterHabitList.ToList().ForEach(x => sickness.Habits.Add(x));

            var filterAdviceList = from x in DbContext.Advice where adviceList.Any(y => y == x.Id) select x;
            sickness.Advice = new List<Advice>();
            filterAdviceList.ToList().ForEach(x => sickness.Advice.Add(x));

            var filterAreaList = from x in DbContext.Areas where areaList.Any(y => y == x.Id) select x;
            sickness.Areas = new List<Area>();
            filterAreaList.ToList().ForEach(x => sickness.Areas.Add(x));

            try
            {
                DbContext.Set<Sickness>().Attach(sickness);
                DbContext.Entry(sickness).State = EntityState.Modified;
                if (DbContext.SaveChanges() > 0)
                {
                    result.ResultCode = ResultCodeOption.Ok;
                    result.Message = DataAccessResult.SuccessDefaultString;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                LogHelper.Log.Error($"{GetType().Name} exception.", dbEx);
                result.ResultCode = ResultCodeOption.ValidationFailure;
                result.Message = dbEx.InnerException?.InnerException?.Message;
            }
            catch (DbUpdateException dbEx)
            {
                LogHelper.Log.Error($"{GetType().Name} exception.", dbEx);
                result.ResultCode = ResultCodeOption.Duplicate;
                result.Message = dbEx.InnerException?.InnerException?.Message;
            }
            catch (Exception dbEx)
            {
                LogHelper.Log.Error($"{GetType().Name} exception.", dbEx);
                result.Message = dbEx.GetType().ToString();
            }

            return result;
        }
    }
}
