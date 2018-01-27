using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using TancleClient.TranslationByMarkupExtension;
using TancleClient.Utility;

namespace TancleClient
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            LogHelper.Instance.Info("Tancle Client application is start!");

            TranslationManager.Instance.TranslationProvider = new ResxTranslationProvider("TancleClient.Properties.Resources", Assembly.GetExecutingAssembly());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LogHelper.Instance.Info("Tancle Client application is stop!");

            DispatcherHelper.Reset();

        }

        public App()
        {
            // 为了在非UI线程调用界面资源，要在此处初始化DispatcherHelper
            DispatcherHelper.Initialize();
        }


    }

    
}
