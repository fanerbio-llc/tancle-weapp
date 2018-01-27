﻿using Microsoft.Practices.ServiceLocation;
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
    public class AreaManagementViewModel : MenuViewModelBase
    {
        #region Private fields

        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        private ObservableCollection<Area> _dataList;
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
        #endregion

        #region Constructors

        public AreaManagementViewModel()
        {
            DataList = new ObservableCollection<Area>()
            {
                new Area {Id=1,AreaName="陕西",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
                new Area {Id=2,AreaName="山西",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
                new Area {Id=3,AreaName="四川",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
            };

        }

        #endregion

        #region Override methods

        public override void DisplayTitle()
        {
            Title = TranslationService.Translate("View_AreaManagement").ToString();
            SearchHintText = TranslationService.Translate("View_AreaManagement_FindHint").ToString();
            Icon =
    "F1 M 38,17.4167C 33.6278,17.4167 30.0833,20.9611 30.0833,25.3333C 30.0833,29.7056 33.6278,33.25 38,33.25C 42.3723,33.25 45.9167,29.7056 45.9167,25.3333C 45.9167,20.9611 42.3722,17.4167 38,17.4167 Z M 30.0833,44.3333L 29.4774,58.036C 32.2927,59.4011 35.4528,60.1667 38.7917,60.1667C 41.5308,60.1667 44.1496,59.6515 46.5564,58.7126L 45.9167,44.3333C 46.9722,44.8611 49.0834,49.0833 49.0834,49.0833C 49.0834,49.0833 50.1389,50.6667 50.6667,57L 55.4166,55.4167L 53.8333,47.5C 53.8333,47.5 50.6667,36.4167 44.3332,36.4168L 31.6666,36.4168C 25.3333,36.4167 22.1667,47.5 22.1667,47.5L 20.5833,55.4166L 25.3333,56.9999C 25.8611,50.6666 26.9167,49.0832 26.9167,49.0832C 26.9167,49.0832 29.0278,44.8611 30.0833,44.3333 Z ";

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
