/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TancleClient"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Autofac;
using Autofac.Extras.CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TancleClient.Service;

namespace TancleClient.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            // Use Autofac as IOC container
            var builder = new ContainerBuilder();

            #region Register relevant service

            builder.RegisterType<TranslationService>().As<ITranslationService>().SingleInstance();

            #endregion

            #region Register viewmodel

            builder.RegisterType<MainViewModel>().As<MainViewModel>().SingleInstance();
            builder.RegisterType<SicknessManagementViewModel>().As<SicknessManagementViewModel>().SingleInstance();
            builder.RegisterType<HabitManagementViewModel>().As<HabitManagementViewModel>().SingleInstance();
            builder.RegisterType<AreaManagementViewModel>().As<AreaManagementViewModel>().SingleInstance();
            builder.RegisterType<AdviceManagementViewModel>().As<AdviceManagementViewModel>().SingleInstance();

            #endregion

            // Perform registrations and build the container. 
            var container = builder.Build();

            // Set the service locator to an AutofacServiceLocator. 
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        



    }
}