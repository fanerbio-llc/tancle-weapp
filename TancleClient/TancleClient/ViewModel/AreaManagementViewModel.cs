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
    public class AreaManagementViewModel : MenuViewModelBase
    {
        #region Private fields

        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();
        private static readonly DataAccessServiceGeneric<TancleConfigDbContext, Area> DataService =
            ServiceLocator.Current.GetInstance<DataAccessServiceGeneric<TancleConfigDbContext, Area>>();

        private ObservableCollection<Area> _dataList;
        private Area _editItem;
        private Area _selectedItem;
        #endregion

        #region Properties bind to view

        public ObservableCollection<Area> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                this.RaisePropertyChanged("DataList");
            }
        }

        public Area SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                this.RaisePropertyChanged("SelectedItem");
            }
        }

        public Area EditItem
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

        public AreaManagementViewModel()
        {
            CreateCommand = new RelayCommand(BtnCreateClick);
            DeleteCommand = new RelayCommand(BtnDeleteClick, () => SelectedItem != null);
            UpdateCommand = new RelayCommand(BtnUpdateClick);
            CancelCommand = new RelayCommand(BtnCancelClick);
            ListItemSelectedChangeCommand = new RelayCommand<Area>(ListItemSelectedChange);
        }

        #endregion

        #region Override methods

        public override void DisplayTitle()
        {
            Title = TranslationService.Translate("View_AreaManagement").ToString();
            SearchHintText = TranslationService.Translate("View_AreaManagement_SearchHint").ToString();
            Icon = "M20.243986,27.24403L27.270979,27.24403C26.90798,28.753034,24.869982,30.072037,21.879986,30.937038z M7.8949959,27.24403L18.894993,27.24403 20.669992,31.250021C18.611992,31.72302 16.210994,32.000019 13.635995,32.000019 10.877995,32.000019 8.314996,31.68502 6.1649964,31.148022z M0,27.24403L6.5459797,27.24403 4.963984,30.815016C2.2039932,29.95002,0.34699893,28.681024,0,27.24403z M13.62799,6.1700138C16.353997,6.1700138 18.564001,8.3800181 18.564001,11.106025 18.564001,13.83203 16.353997,16.042036 13.62799,16.042036 10.901985,16.042036 8.6919801,13.83203 8.6919801,11.106025 8.6919801,8.3800181 10.901985,6.1700138 13.62799,6.1700138z M13.627994,4.9359745C10.226017,4.9359745 7.4580128,7.7039803 7.4580128,11.105988 7.4580128,14.507997 10.226017,17.276003 13.627994,17.276003 17.030001,17.276003 19.798006,14.507997 19.798006,11.105988 19.798006,7.7039803 17.030001,4.9359745 13.627994,4.9359745z M13.627994,0C19.752016,0 24.73401,4.981995 24.73401,11.105988 24.73401,13.68799 23.841005,16.060976 22.356996,17.948976 22.356996,17.948976 21.446993,18.994997 20.968019,19.418001L18.616386,21.872347 18.705789,21.885271C22.154587,22.412216,24.911417,23.460829,26.315977,24.776018L15.834232,24.776018 13.642002,27.063999 11.442275,24.776018 0.95499957,24.776018C2.3595611,23.460829,5.1163907,22.412216,8.5655129,21.885271L8.6511319,21.872894 6.3130236,19.440981C5.8270011,19.013979 4.9050043,17.957003 4.905004,17.957003 3.418005,16.067995 2.5220092,13.691988 2.522009,11.105988 2.5220092,4.981995 7.5040028,0 13.627994,0z";
        }

        public override void UpdateDataList(int pageIndex, out int total, int itemPerPage = 10)
        {
            DataList?.Clear();

            IEnumerable<Area> tempList;

            var searcher = ServiceLocator.Current.GetInstance<IViewModelSearcher>();
            if (searcher.Search())
            {
                var searchText = searcher.GetSearchText();
                tempList = DataService.LoadPageTuples(
                    pageIndex,
                    itemPerPage,
                    out total,
                    x => x.AreaName.Contains(searchText),
                    true,
                    x => x.Id);
            }
            else
            {
                tempList = DataService.LoadPageTuples(
                    pageIndex,
                    itemPerPage,
                    out total,
                    x => x.Id > 0,
                    true,
                    x => x.Id);
            }

            if (tempList == null)
            {
                DataList = null;
                return;
            }

            DataList = new ObservableCollection<Area>(tempList);
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
            placeHolder.AppendLine($"{TranslationService.Translate("View_AreaManagement_AreaName")}: {{0}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_Header_CreatedTime")}: {{1}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_Header_UpdatedTime")}: {{2}}");
            return placeHolder.ToString();
        }

        private void BtnCreateClick()
        {
            SelectedItem = null;
            DeselectItemInDataList();
            EditItem = new Area();
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

        private void ListItemSelectedChange(Area area)
        {
            SelectedItem = area;
            EditItem = area?.Clone() as Area;
        }
        #endregion
    }
}
