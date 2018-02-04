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
    public class SicknessManagementViewModel : MenuViewModelBase
    {
        #region Private fields

        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        private static readonly DataAccessServiceGeneric<TancleConfigDbContext, Sickness> DataService =
            ServiceLocator.Current.GetInstance<DataAccessServiceGeneric<TancleConfigDbContext, Sickness>>();

        private ObservableCollection<Sickness> _dataList;
        private Sickness _editItem;
        private Sickness _selectedItem;
        #endregion

        #region Properties bind to view

        public ObservableCollection<Sickness> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                this.RaisePropertyChanged("DataList");
            }
        }

        public Sickness SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                this.RaisePropertyChanged("SelectedItem");
            }
        }

        public Sickness EditItem
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

        public SicknessManagementViewModel()
        {
            CreateCommand = new RelayCommand(BtnCreateClick);
            DeleteCommand = new RelayCommand(BtnDeleteClick, () => SelectedItem != null);
            UpdateCommand = new RelayCommand(BtnUpdateClick);
            CancelCommand = new RelayCommand(BtnCancelClick);
            ListItemSelectedChangeCommand = new RelayCommand<Sickness>(ListItemSelectedChange);
        }
        #endregion

        #region Override methods

        public override void DisplayTitle()
        {
            Title = TranslationService.Translate("View_SicknessManagement").ToString();
            SearchHintText = TranslationService.Translate("View_SicknessManagement_SearchHint").ToString();
            Icon = "M8.99124145507813,8.35562705993652L8.99124145507813,10.8775024414063 6.46875381469727,10.8775024414063 6.46875381469727,12.8956270217896 8.99124145507813,12.8956270217896 8.99124145507813,15.4181108474731 11.0087585449219,15.4181108474731 11.0087585449219,12.8956270217896 13.5312461853027,12.8956270217896 13.5312461853027,10.8775024414063 11.0087585449219,10.8775024414063 11.0087585449219,8.35562705993652 8.99124145507813,8.35562705993652z M0,7.31436824798584L20,7.31436824798584 20,16.4599990844727 0,16.4599990844727 0,7.31436824798584z M0,0L8.05624008178711,0 8.05624008178711,2.06001043319702 20,2.06001043319702 20,4.55375146865845 0,4.55375146865845 0,2.46812558174133 0,2.06001043319702 0,0z";
        }

        public override void UpdateDataList(int pageIndex, out int total, int itemPerPage = 10)
        {
            DataList?.Clear();

            IEnumerable<Sickness> tempList;

            var searcher = ServiceLocator.Current.GetInstance<IViewModelSearcher>();
            if (searcher.Search())
            {
                var searchText = searcher.GetSearchText();
                tempList = DataService.LoadPageTuplesWithRelatedTuples
                    <string, ICollection<Habit>, ICollection<Advice>, ICollection<Area>, string, string> (
                    pageIndex,
                    itemPerPage,
                    out total,
                    x => x.SicknessName.Contains(searchText),
                    true,
                    x => x.SicknessNo,
                    x => x.Habits,
                    x => x.Advice,
                    x => x.Areas);
            }
            else
            {
                tempList = DataService.LoadPageTuplesWithRelatedTuples
                    <string, ICollection<Habit>, ICollection<Advice>, ICollection<Area>, string, string>(
                    pageIndex,
                    itemPerPage,
                    out total,
                    x => x.Id > 0,
                    true,
                    x => x.SicknessNo,
                    x => x.Habits,
                    x => x.Advice,
                    x => x.Areas);
            }

            if (tempList == null)
            {
                DataList = null;
                return;
            }

            DataList = new ObservableCollection<Sickness>(tempList);
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
            placeHolder.AppendLine($"{TranslationService.Translate("View_SicknessManagement_SicknessNo")}: {{0}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_SicknessManagement_SicknessName")}: {{1}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_Header_CreatedTime")}: {{2}}");
            placeHolder.AppendLine($"{TranslationService.Translate("View_Header_UpdatedTime")}: {{3}}");
            return placeHolder.ToString();
        }

        private void BtnCreateClick()
        {
            SelectedItem = null;
            DeselectItemInDataList();
            EditItem = new Sickness();
            ServiceLocator.Current.GetInstance<HabitSicknessViewModel>().ResetItems();
            ServiceLocator.Current.GetInstance<AdviceSicknessViewModel>().ResetItems();
            ServiceLocator.Current.GetInstance<AreaSicknessViewModel>().ResetItems();
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
                BtnCancelClick();
                ServiceLocator.Current.GetInstance<IViewModelPaginator>().UpdateDataList(this);
            }

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
                    ServiceLocator.Current.GetInstance<IViewModelUpdateSickness>().AddSickness(
                        sickness: EditItem,
                        popUpText: EditItem.ToString(GetPlaceHolder()),
                        popUpConfirm: true)
                    : ServiceLocator.Current.GetInstance<IViewModelUpdateSickness>().UpdateSickness(
                        sickness: EditItem,
                        popUpText: EditItem.ToString(GetPlaceHolder()),
                        popUpConfirm: true);

                if (result)
                {
                    BtnCancelClick();
                    ServiceLocator.Current.GetInstance<IViewModelPaginator>().UpdateDataList(this);
                }
            }
        }

        private void BtnCancelClick()
        {
            SelectedItem = null;
            DeselectItemInDataList();
            EditItem = null;
            ServiceLocator.Current.GetInstance<HabitSicknessViewModel>().ResetItems();
            ServiceLocator.Current.GetInstance<AdviceSicknessViewModel>().ResetItems();
            ServiceLocator.Current.GetInstance<AreaSicknessViewModel>().ResetItems();
        }

        private void ListItemSelectedChange(Sickness sickness)
        {
            SelectedItem = sickness;
            EditItem = sickness?.Clone() as Sickness;

            if (EditItem != null)
            {
                ServiceLocator.Current.GetInstance<HabitSicknessViewModel>().SetCheckedItems(new List<Habit>(SelectedItem.Habits));
                ServiceLocator.Current.GetInstance<AdviceSicknessViewModel>().SetCheckedItems(new List<Advice>(SelectedItem.Advice));
                ServiceLocator.Current.GetInstance<AreaSicknessViewModel>().SetCheckedItems(new List<Area>(SelectedItem.Areas));
            }
        }
        #endregion
    }
}
