using BaseCommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Model;

namespace TancleDataModel
{
    public class TancleConfigDbContext : DbContext
    {
        public TancleConfigDbContext() : base("name=TancleConfigDbContext")
        {
            // set command executing timeout to 5 seconds.
            Database.CommandTimeout = 5;
        }

        public virtual DbSet<Sickness> Sicknesses { get; set; }

        public virtual DbSet<Habit> Habits { get; set; }

        public virtual DbSet<Area> Areas { get; set; }

        public virtual DbSet<Advice> Advice { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Sickness>()
            //    .HasOptional(t=>t.Habits)
        }
    }

    public class TancleConfigDbContextInitializer: DropCreateDatabaseIfModelChanges<TancleConfigDbContext>
    {
        protected override void Seed(TancleConfigDbContext context)
        {
            LogHelper.Log.Info("Initialize tancle.config database with seed configuration");
            base.Seed(context);

            AddDefaultSicknessWithHabitWithAreaWithAdvice(context);
        }

        private void AddDefaultSicknessWithHabitWithAreaWithAdvice(TancleConfigDbContext context)
        {
            var habit = new Habit { Id = 1, HabitNo = "B001", HabitName = "吸烟", CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now };
            var area = new Area { Id = 1, AreaName = "陕西", CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now };
            var advice = new Advice { Id = 1, AdviceNo = "C001", AdviceName = "戒烟", CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now };

            context.Habits.Add(habit);
            context.Areas.Add(area);
            context.Advice.Add(advice);

            var sicknessList = new List<Sickness>
            {
                new Sickness
                {
                    Id = 1,
                    SicknessNo = "A001",
                    SicknessName = "糖尿病",
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    Habits = new List<Habit> { habit },
                    Areas = new List<Area> { area },
                    Advice = new List<Advice> { advice }
                },
                new Sickness
                {
                    Id = 1,
                    SicknessNo = "A002",
                    SicknessName = "肺癌",
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                },
                new Sickness
                {
                    Id = 1,
                    SicknessNo = "A003",
                    SicknessName = "肾衰竭",
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                }
            };

            context.Sicknesses.AddRange(sicknessList);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                LogHelper.Log.Error(e.Message, e);
            }
        }
    }
}
