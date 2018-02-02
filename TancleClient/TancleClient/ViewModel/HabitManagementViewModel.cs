using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TancleClient.Service;
using TancleClient.ViewModel.Interface;
using TancleDataModel;
using TancleDataModel.Implementation;
using TancleDataModel.Model;

namespace TancleClient.ViewModel
{
    public class HabitManagementViewModel : MenuViewModelBase
    {
        #region Private fields

        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        private static readonly DataAccessServiceGeneric<TancleConfigDbContext, Habit> DataService = 
            ServiceLocator.Current.GetInstance<DataAccessServiceGeneric<TancleConfigDbContext, Habit>>();

        private ObservableCollection<Habit> _dataList;
        private Habit _selectedItem;
        private Habit _editItem;
        #endregion

        #region Properties bind to view

        public ObservableCollection<Habit> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                this.RaisePropertyChanged("DataList");
            }
        }

        public Habit SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                this.RaisePropertyChanged("SelectedItem");
            }
        }

        public Habit EditItem
        {
            get { return _editItem; }
            set
            {
                _editItem = value;
                this.RaisePropertyChanged("EditItem");
            }
        }
        #endregion

        #region Constructors

        public HabitManagementViewModel()
        {
            CreateCommand = new RelayCommand(BtnCreateClick);
            DeleteCommand = new RelayCommand(BtnDeleteClick, () => SelectedItem != null);
            UpdateCommand = new RelayCommand(BtnUpdateClick);
            CancelCommand = new RelayCommand(BtnCancelClick);
            ListItemSelectedChangeCommand = new RelayCommand<Habit>(ListItemSelectedChange);
        }

        #endregion

        #region Override methods

        public override void DisplayTitle()
        {
            Title = TranslationService.Translate("View_HabitManagement").ToString();
            SearchHintText = TranslationService.Translate("View_HabitManagement_SearchHint").ToString();
            Icon = "M8.12500953674316,6.24998331069946L8.56711864471436,6.43310880661011 8.75000953674316,6.87498378753662 8.75000953674316,11.2499847412109 11.8750095367432,11.2499847412109 12.3171186447144,11.4331092834473 12.5000095367432,11.8749847412109 12.3171186447144,12.3168592453003 11.8750095367432,12.4999847412109 7.50000905990601,12.4999847412109 7.50000905990601,6.87498378753662 7.68289947509766,6.43310880661011 8.12500953674316,6.24998331069946z M8.12500095367432,4.99999904632568L5.45161914825439,5.54116106033325 3.26602244377136,7.01601457595825 1.79116463661194,9.20161056518555 1.25,11.875 1.79116463661194,14.5483894348145 3.26602244377136,16.7339859008789 5.45161914825439,18.2088375091553 8.12500095367432,18.75 10.7983903884888,18.2088375091553 12.9839859008789,16.7339859008789 14.4588394165039,14.5483894348145 15.0000019073486,11.875 14.4588394165039,9.20161056518555 12.9839859008789,7.01601457595825 10.7983903884888,5.54116106033325 8.12500095367432,4.99999904632568z M7.1881217956543,1.25000011920929L7.1881217956543,1.875 9.0631217956543,1.875 9.0631217956543,1.25000011920929 7.1881217956543,1.25000011920929z M5.9381217956543,0L10.3131217956543,0 10.3131217956543,3.12500023841858 8.7231330871582,3.12500023841858 8.7231330871582,3.77207922935486 8.74920654296875,3.77373743057251 11.6886529922485,4.57400989532471 14.07066822052,6.34304189682007 15.6671514511108,8.85273742675781 16.2500019073486,11.875 15.6104707717896,15.0345230102539 13.8675012588501,17.6174926757813 11.2845325469971,19.360466003418 8.12500095367432,20 4.96546936035156,19.360466003418 2.38250041007996,17.6174926757813 0.639531314373016,15.0345230102539 0,11.875 0.564499914646149,8.89877319335938 2.11328911781311,6.41482830047607 4.42930173873901,4.64022588729858 7.29547214508057,3.79202771186829 7.4731330871582,3.77620100975037 7.4731330871582,3.12500023841858 5.9381217956543,3.12500023841858 5.9381217956543,0z";
        }

        public override void UpdateDataList(int pageIndex, out int total, int itemPerPage = 10)
        {
            DataList?.Clear();

            IEnumerable<Habit> tempList;

            var searcher = ServiceLocator.Current.GetInstance<IViewModelSearcher>();
            if (searcher.Search())
            {
                var searchText = searcher.GetSearchText();
                tempList = DataService.LoadPageTuples(
                    pageIndex,
                    itemPerPage,
                    out total,
                    x => x.HabitName.Contains(searchText),
                    true,
                    x => x.HabitNo);
            }
            else
            {
                tempList = DataService.LoadPageTuples(
                    pageIndex, 
                    itemPerPage, 
                    out total, 
                    x => x.Id > 0, 
                    true, 
                    x => x.HabitNo);
            }

            if (tempList == null)
            {
                DataList = null;
                return;
            }

            DataList = new ObservableCollection<Habit>(tempList);
        }
        #endregion

        #region Command definitions

        public ICommand CreateCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public ICommand UpdateCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        public ICommand ListItemSelectedChangeCommand { get; private set; }
        #endregion

        #region Commmand implementations

        private void DeselectItemInDataList()
        {
            var tempList = DataList;
            DataList = null;
            DataList = tempList;
        }

        private string GetPlaceHolder()
        {
            StringBuilder placeHolder = new StringBuilder();
            placeHolder.AppendLine($"{TranslationService.Translate("View_HabitManagement_HabitNo")}: {{0}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_HabitManagement_HabitName")}: {{1}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_Header_CreatedTime")}: {{2}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_Header_UpdatedTime")}: {{3}}");
            return placeHolder.ToString();
        }

        private void BtnCreateClick()
        {
            SelectedItem = null;
            DeselectItemInDataList();
            EditItem = new Habit();
        }

        private void BtnDeleteClick()
        {
            if (EditItem == null)
            {
                return;
            }

            if (ServiceLocator.Current.GetInstance<IViewModelDeleteItem>().Delete(
                dataService: DataService,
                entityId: EditItem.Id,
                popUpText: EditItem.ToString(GetPlaceHolder()),
                popUpConfirm: true))
            {
                SelectedItem = null;
                EditItem = null;
            }

            ServiceLocator.Current.GetInstance<IViewModelPaginator>().UpdateDataList(this);
        }

        private void BtnUpdateClick()
        {
            if (EditItem == null)
            {
                return;
            }

            if (EditItem.Id == 0)
            {
                // Create new item, add create time and update time
                EditItem.CreatedTime = DateTime.Now;
                EditItem.UpdatedTime = DateTime.Now;
            }
            else {
                // Update exsist data, modify update time only
                EditItem.UpdatedTime = DateTime.Now;
            }

            if (ServiceLocator.Current.GetInstance<IViewModelValidateData>().Validate(EditItem))
            {
                var result = EditItem.Id == 0 ?
                    ServiceLocator.Current.GetInstance<IViewModelAddItem>().Add(
                        dataService: DataService,
                        entity: EditItem,
                        popUpText: EditItem.ToString(GetPlaceHolder()),
                        popUpConfirm: true)
                    : ServiceLocator.Current.GetInstance<IViewModelUpdateItem>().Update(
                        dataService: DataService,
                        entityId: EditItem.Id,
                        popUpText: EditItem.ToString(GetPlaceHolder()),
                        copyImpl: EditItem,
                        popUpConfirm: true);

                if (result)
                {
                    EditItem = null;
                }
            }

            ServiceLocator.Current.GetInstance<IViewModelPaginator>().UpdateDataList(this);
        }

        private void BtnCancelClick()
        {
            SelectedItem = null;
            DeselectItemInDataList();
            EditItem = null;
        }

        private void ListItemSelectedChange(Habit habit)
        {
            SelectedItem = habit;
            EditItem = habit?.Clone() as Habit;
        }
        #endregion
    }
}
