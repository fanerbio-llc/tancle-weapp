using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using TancleClient.Service;
using TancleClient.ViewModel.Interface;

namespace TancleClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IViewModelGetSelectedView
    {
        #region Private fields

        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        private List<MenuViewModelBase> _viewsOfConfig = new List<MenuViewModelBase>();
        private List<MenuViewModelBase> _viewsOfRunning = new List<MenuViewModelBase>();
        private MenuViewModelBase _selectView;
        #endregion

        #region Properties bind to view

        public List<MenuViewModelBase> ViewsOfConfig
        {
            get { return _viewsOfConfig; }
            set
            {
                _viewsOfConfig = value;
                this.RaisePropertyChanged("ViewsOfConfig");
            }
        }

        public List<MenuViewModelBase> ViewsOfRunning
        {
            get { return _viewsOfRunning; }
            set
            {
                _viewsOfRunning = value;
                this.RaisePropertyChanged("ViewsOfRunning");
            }
        }

        public MenuViewModelBase SelectView
        {
            get { return _selectView; }
            set
            {
                _selectView = value;
                this.RaisePropertyChanged("SelectView");
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ViewsOfConfig.Add(ServiceLocator.Current.GetInstance<SicknessManagementViewModel>());
            ViewsOfConfig.Add(ServiceLocator.Current.GetInstance<HabitManagementViewModel>());
            ViewsOfConfig.Add(ServiceLocator.Current.GetInstance<AreaManagementViewModel>());
            ViewsOfConfig.Add(ServiceLocator.Current.GetInstance<AdviceManagementViewModel>());

            ViewsOfConfig.ForEach(vm => vm.DisplayTitle());

            ViewSelectedChangeCommand = new RelayCommand<MenuViewModelBase>(ViewSelectedChange);

            // Set specific view
            SelectView = ServiceLocator.Current.GetInstance<SicknessManagementViewModel>();

        }
        #endregion

        #region Command definitions

        public ICommand ViewSelectedChangeCommand { get; private set; }
        #endregion

        #region Commmand implementations

        private void ViewSelectedChange(MenuViewModelBase menuView)
        {
            // DO NOT mess up below steps.

            // step #0: skip unselected menu event when switch menu of different menu group
            if (menuView == null) return;

            // step #1: configure searcher

            var searcher = ServiceLocator.Current.GetInstance<IViewModelSearcher>();
            searcher.ResetSearchText();

            // step #2: configure paginator

            var paginator = ServiceLocator.Current.GetInstance<IViewModelPaginator>();
            paginator.ResetDisplayPage();

            // if query database failure, prompt

            if (SelectView != null && SelectView.QueryDatabaseFailure())
            {
                ServiceLocator.Current.GetInstance<IViewModelErrorWindow>().ErrorWindow(
                    this,
                    TranslationService.Translate("View_Messagebox_Text_QueryDatabaseFailure").ToString());
            }
        }
        #endregion

        #region Interface implementations

        public MenuViewModelBase GetSelectedView()
        {
            return SelectView;
        } 
        #endregion
    }
}