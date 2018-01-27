using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleClient.Service;
using TancleDataModel.Model;

namespace TancleClient.ViewModel
{
    public class SicknessManagementViewModel : MenuViewModelBase
    {
        #region Private fields

        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        private ObservableCollection<Sickness> _dataList;
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
        #endregion

        #region Constructors

        public SicknessManagementViewModel()
        {
            //DataList = new ObservableCollection<Sickness>();
            DataList = new ObservableCollection<Sickness>()
            {
                new Sickness {Id=1,SicknessNo="A001",SicknessName="糖尿病",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
                new Sickness {Id=2,SicknessNo="A002",SicknessName="肺癌",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
                new Sickness {Id=3,SicknessNo="A003",SicknessName="肾衰竭",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
            };

        }
        #endregion

        #region Override methods

        public override void DisplayTitle()
        {
            Title = TranslationService.Translate("View_SicknessManagement").ToString();
            SearchHintText = TranslationService.Translate("View_SicknessManagement_FindHint").ToString();
            Icon =
    "F1 M 51.0065,41.0057C 54.3207,41.0057 57.0074,43.3939 57.0074,46.3398C 57.0074,46.8003 56.9418,47.2471 56.8183,47.6733L 51.0065,57.008L 45.1948,47.6733C 45.0714,47.2471 45.0057,46.8003 45.0057,46.3398C 45.0057,43.3939 47.6924,41.0057 51.0065,41.0057 Z M 51.0065,44.6729C 50.0859,44.6729 49.3396,45.4192 49.3396,46.3398C 49.3396,47.2604 50.0859,48.0067 51.0065,48.0067C 51.9272,48.0067 52.6734,47.2604 52.6734,46.3398C 52.6734,45.4192 51.9272,44.6729 51.0065,44.6729 Z M 24.0033,56.0078L 24.0033,38.0053L 22.0031,40.0056L 19.0027,35.0049L 38.0053,20.0028L 45.0063,25.5299L 45.0063,21.753L 49.0068,21.0029L 49.0068,28.6882L 57.0079,35.0049L 54.2034,39.6791L 53.4199,39.4179L 52.0073,38.0053L 52.0073,39.1438L 51.0065,39.0887C 50.3161,39.0887 49.646,39.1664 49.0068,39.3126L 49.0068,36.005L 38.0053,26.9205L 27.0038,36.005L 27.0038,53.0074L 33.0046,53.0074L 33.0046,42.006L 43.006,42.006L 43.006,46.283L 43.006,53.0074L 46.3883,53.0074L 48.2564,56.0078L 24.0033,56.0078 Z ";

        }

        public override void UpdateDataList(int pageIndex, out int total, int itemPerPage = 10)
        {
            DataList?.Clear();
            SelectedItem = null;

            total = 0;

        }

        #endregion
    }
}
