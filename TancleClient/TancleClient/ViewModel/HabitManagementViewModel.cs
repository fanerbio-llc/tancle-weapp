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
    public class HabitManagementViewModel : MenuViewModelBase
    {
        #region Private fields

        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        private ObservableCollection<Habit> _dataList;
        private Habit _selectedItem;
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
        #endregion

        #region Constructors

        public HabitManagementViewModel()
        {
            DataList = new ObservableCollection<Habit>()
            {
                new Habit {Id=1,HabitNo="B001",HabitName="吸烟",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
                new Habit {Id=2,HabitNo="B002",HabitName="饮酒",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
                new Habit {Id=3,HabitNo="B003",HabitName="槟榔",CreatedTime=DateTime.Now,UpdatedTime=DateTime.Now },
            };
        }
        #endregion

        #region Override methods

        public override void DisplayTitle()
        {
            Title = TranslationService.Translate("View_HabitManagement").ToString();
            SearchHintText = TranslationService.Translate("View_HabitManagement_FindHint").ToString();
            Icon =
    "F1 M 38,17C 40.9455,17 43.3333,19.3878 43.3333,22.3333C 43.3333,25.2788 40.9455,27.6667 38,27.6667C 35.0545,27.6667 32.6667,25.2788 32.6667,22.3333C 32.6667,19.3878 35.0545,17 38,17 Z M 32.6666,34.3834C 31.9555,34.7389 30.7833,37.8333 31.4262,38.2501L 27.964,37.6132C 30.3193,36.76 30.7911,35.3344 30.9823,32.7335L 30.8009,31.1163C 31.5744,30.4725 32.7185,29.0501 33.7333,29.0502L 42.2666,29.0502C 43.3037,29.0501 44.2149,29.4913 44.9999,30.1593L 45.4999,32.0001C 45.4999,34.1736 47.1556,34.8271 48.886,35.8798L 46.4666,35.8292C 45.8376,35.8068 45.2551,35.9483 44.7188,36.2059C 44.2645,35.4252 43.7029,34.5682 43.3333,34.3834L 43.4534,37.0835C 41.1956,39.1569 40.0666,43.0679 40.0666,43.0679L 39.6764,45.0053L 38.5333,45.05C 37.7661,45.05 37.0129,44.99 36.2782,44.8745C 35.6933,43.3208 34.4183,40.5162 32.4533,39.2079L 32.6666,34.3834 Z M 24.7333,26.95C 27.6789,26.95 30.0667,29.3378 30.0667,32.2833C 30.0667,35.2288 27.6789,37.6167 24.7333,37.6167C 21.7878,37.6167 19.4,35.2288 19.4,32.2833C 19.4,29.3378 21.7878,26.95 24.7333,26.95 Z M 19.4,44.3333C 18.6889,44.6889 17.2667,47.5333 17.2667,47.5333C 17.2667,47.5333 16.5556,48.6 16.2,52.8666L 13,51.8L 14.0667,46.4667C 14.0667,46.4667 16.2,39 20.4666,39.0001L 28.9999,39.0001C 33.2667,39 35.4,46.4667 35.4,46.4667L 36.4666,51.8L 33.2667,52.8667C 32.9111,48.6 32.2001,47.5333 32.2001,47.5333C 32.2001,47.5333 30.7778,44.6889 30.0667,44.3333L 30.4976,54.0204C 28.8762,54.6529 27.112,55 25.2667,55C 23.0173,55 20.8884,54.4843 18.9918,53.5646L 19.4,44.3333 Z M 51.7333,24.931C 54.6788,25.0359 57.0667,27.5087 57.0667,30.4542C 57.0667,33.3997 54.6788,35.7025 51.7333,35.5977C 48.7878,35.4928 46.4,33.02 46.4,30.0745C 46.4,27.129 48.7878,24.8262 51.7333,24.931 Z M 46.4,42.1245C 45.6889,42.4547 44.2667,45.2485 44.2667,45.2485C 44.2667,45.2485 43.5556,46.2898 43.2,50.5438L 40,49.3632L 41.0667,44.0679C 41.0667,44.0679 43.2,36.6772 47.4666,36.8292L 55.9999,37.133C 60.2667,37.2848 62.4,44.8274 62.4,44.8274L 63.4666,50.1988L 60.2667,51.1515C 59.9111,46.8722 59.2001,45.7802 59.2001,45.7802C 59.2001,45.7802 57.7778,42.8851 57.0667,42.5042L 57.4976,52.2067C 55.8762,52.7814 54.112,53.0657 52.2667,53C 50.0173,52.9199 47.8884,52.3284 45.9918,51.3412L 46.4,42.1245 Z ";

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