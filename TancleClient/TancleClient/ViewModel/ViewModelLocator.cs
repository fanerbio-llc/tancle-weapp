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
using MvvmDialogs;
using TancleClient.Service;
using TancleClient.ViewModel.Implementation;
using TancleClient.ViewModel.Interface;
using TancleDataModel;
using TancleDataModel.Implementation;
using TancleDataModel.Interface;
using TancleDataModel.Model;

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
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            #endregion

            #region Register viewmodel

            // Views
            builder.RegisterType<MainViewModel>()
                .As<MainViewModel>()
                .As<IViewModelGetSelectedView>()
                .SingleInstance();

            builder.RegisterType<SicknessManagementViewModel>().As<SicknessManagementViewModel>().SingleInstance();
            builder.RegisterType<HabitManagementViewModel>().As<HabitManagementViewModel>().SingleInstance();
            builder.RegisterType<AreaManagementViewModel>().As<AreaManagementViewModel>().SingleInstance();
            builder.RegisterType<AdviceManagementViewModel>().As<AdviceManagementViewModel>().SingleInstance();

            // Components
            builder.RegisterType<SearcherViewModel>().As<SearcherViewModel>().As<IViewModelSearcher>().SingleInstance();
            builder.RegisterType<PaginatorViewModel>().As<PaginatorViewModel>().As<IViewModelPaginator>().SingleInstance();

            builder.RegisterType<ViewModelPopUpWindowImpl>()
                .As<IViewModelConfirmWindow>()
                .As<IViewModelHintWindow>()
                .As<IViewModelErrorWindow>()
                .SingleInstance();

            builder.RegisterType<ViewModelItemOperationImpl>()
                .As<IViewModelAddItem>()
                .As<IViewModelUpdateItem>()
                .As<IViewModelDeleteItem>()
                .As<IViewModelValidateData>()
                .SingleInstance();
            #endregion

            #region Register data access service

            builder.RegisterType<GetTancleConfigDbContextImpl>().As<IGetDbContext<TancleConfigDbContext>>().SingleInstance();

            builder.RegisterType<DataAccessServiceGeneric<TancleConfigDbContext, Sickness>>().SingleInstance();
            builder.RegisterType<DataAccessServiceGeneric<TancleConfigDbContext, Habit>>().SingleInstance();
            builder.RegisterType<DataAccessServiceGeneric<TancleConfigDbContext, Advice>>().SingleInstance();
            builder.RegisterType<DataAccessServiceGeneric<TancleConfigDbContext, Area>>().SingleInstance();
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

        public SearcherViewModel Searcher
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SearcherViewModel>();
            }
        }

        public PaginatorViewModel Paginator
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PaginatorViewModel>();
            }
        }
    }
}