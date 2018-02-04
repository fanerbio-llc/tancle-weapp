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
    public class AdviceManagementViewModel: MenuViewModelBase
    {
        #region Private fields

        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        private static readonly DataAccessServiceGeneric<TancleConfigDbContext, Advice> DataService =
            ServiceLocator.Current.GetInstance<DataAccessServiceGeneric<TancleConfigDbContext, Advice>>();

        private ObservableCollection<Advice> _dataList;
        private Advice _editItem;
        private Advice _selectedItem;
        #endregion

        #region Properties bind to view

        public ObservableCollection<Advice> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                this.RaisePropertyChanged("DataList");
            }
        }

        public Advice SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                this.RaisePropertyChanged("SelectedItem");
            }
        }

        public Advice EditItem
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

        public AdviceManagementViewModel()
        {
            CreateCommand = new RelayCommand(BtnCreateClick);
            DeleteCommand = new RelayCommand(BtnDeleteClick, () => SelectedItem != null);
            UpdateCommand = new RelayCommand(BtnUpdateClick);
            CancelCommand = new RelayCommand(BtnCancelClick);
            ListItemSelectedChangeCommand = new RelayCommand<Advice>(ListItemSelectedChange);
        }

        #endregion

        #region Override methods

        public override void DisplayTitle()
        {
            Title = TranslationService.Translate("View_AdviceManagement").ToString();
            SearchHintText = TranslationService.Translate("View_AdviceManagement_SearchHint").ToString();
            Icon = "M13,26.031L13,28.031 30,28.031 30,26.031z M12,24.031L31,24.031C31.552,24.031,32,24.479,32,25.031L32,29.031C32,29.583,31.552,30.031,31,30.031L12,30.031C11.448,30.031,11,29.583,11,29.031L11,25.031C11,24.479,11.448,24.031,12,24.031z M3,24.031C4.6570001,24.031 6,25.374001 6,27.031 6,28.688 4.6570001,30.031 3,30.031 1.3430001,30.031 3.6872521E-08,28.688 0,27.031 3.6872521E-08,25.374001 1.3430001,24.031 3,24.031z M13,14.031L13,16.031 30,16.031 30,14.031z M12,12.031L31,12.031C31.552,12.031,32,12.479,32,13.031L32,17.031C32,17.583,31.552,18.031,31,18.031L12,18.031C11.448,18.031,11,17.583,11,17.031L11,13.031C11,12.479,11.448,12.031,12,12.031z M3,12.014995C4.6570001,12.014995 6,13.357994 6,15.014995 6,16.671995 4.6570001,18.014994 3,18.014994 1.3430001,18.014994 3.6872521E-08,16.671995 0,15.014995 3.6872521E-08,13.357994 1.3430001,12.014995 3,12.014995z M13,2.0310002L13,4.0310002 30,4.0310002 30,2.0310002z M12,0.031000138L31,0.031000138C31.552,0.03100003,32,0.47900003,32,1.0310002L32,5.0310002C32,5.5830002,31.552,6.0310002,31,6.0310002L12,6.0310002C11.448,6.0310002,11,5.5830002,11,5.0310002L11,1.0310002C11,0.47900003,11.448,0.03100003,12,0.031000138z M3,0C4.6569977,3.0278244E-08 6,1.3430023 6,3 6,4.6569977 4.6569977,6 3,6 1.3430024,6 3.6872521E-08,4.6569977 0,3 3.6872521E-08,1.3430023 1.3430024,3.0278244E-08 3,0z";
        }

        public override void UpdateDataList(int pageIndex, out int total, int itemPerPage = 10)
        {
            DataList?.Clear();

            IEnumerable<Advice> tempList;

            var searcher = ServiceLocator.Current.GetInstance<IViewModelSearcher>();
            if (searcher.Search())
            {
                var searchText = searcher.GetSearchText();
                tempList = DataService.LoadPageTuples(
                    pageIndex,
                    itemPerPage,
                    out total,
                    x => x.AdviceName.Contains(searchText),
                    true,
                    x => x.AdviceNo);
            }
            else
            {
                tempList = DataService.LoadPageTuples(
                    pageIndex,
                    itemPerPage,
                    out total,
                    x => x.Id > 0,
                    true,
                    x => x.AdviceNo);
            }

            if (tempList == null)
            {
                DataList = null;
                return;
            }

            DataList = new ObservableCollection<Advice>(tempList);
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
            placeHolder.AppendLine($"{TranslationService.Translate("View_AdviceManagement_AdviceNo")}: {{0}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_AdviceManagement_AdviceName")}: {{1}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_Header_CreatedTime")}: {{2}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_Header_UpdatedTime")}: {{3}}");
            return placeHolder.ToString();
        }

        private void BtnCreateClick()
        {
            SelectedItem = null;
            DeselectItemInDataList();
            EditItem = new Advice();
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
            else
            {
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
                        copyImpl: EditItem,
                        popUpText: EditItem.ToString(GetPlaceHolder()),
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

        private void ListItemSelectedChange(Advice advice)
        {
            SelectedItem = advice;
            EditItem = advice?.Clone() as Advice;
        }
        #endregion
    }
}
