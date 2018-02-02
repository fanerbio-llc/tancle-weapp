using BaseCommonUtils.Common;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using TancleClient.TranslationByMarkupExtension;
using TancleDataModel;

namespace TancleClient
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            LogHelper.Log.Info("Tancle Client application is start!");

            TranslationManager.Instance.TranslationProvider = new ResxTranslationProvider("TancleClient.Properties.Resources", Assembly.GetExecutingAssembly());

            try
            {
                Database.SetInitializer(new TancleConfigDbContextInitializer());
                using (var db = new TancleConfigDbContext())
                {
                    db.Database.Initialize(true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("Initialize tancle.config database fails", ex);
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LogHelper.Log.Info("Tancle Client application is stop!");

            DispatcherHelper.Reset();

        }

        public App()
        {
            // 为了在非UI线程调用界面资源，要在此处初始化DispatcherHelper
            DispatcherHelper.Initialize();
        }


    }

    
}
