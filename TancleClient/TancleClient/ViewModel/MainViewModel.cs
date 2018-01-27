using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;

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
    public class MainViewModel : ViewModelBase
    {
        private List<MenuViewModelBase> _viewsOfConfig = new List<MenuViewModelBase>();
        private List<MenuViewModelBase> _viewsOfRunning = new List<MenuViewModelBase>();
        private MenuViewModelBase _selectView;

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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ViewsOfConfig.Add(ServiceLocator.Current.GetInstance<SicknessManagementViewModel>());
            ViewsOfConfig.Add(ServiceLocator.Current.GetInstance<HabitManagementViewModel>());
            ViewsOfConfig.Add(ServiceLocator.Current.GetInstance<AreaManagementViewModel>());
            ViewsOfConfig.Add(ServiceLocator.Current.GetInstance<AdviceManagementViewModel>());

            SelectView = ServiceLocator.Current.GetInstance<SicknessManagementViewModel>();

            ViewsOfConfig.ForEach(vm => vm.DisplayTitle());

        }
    }
}