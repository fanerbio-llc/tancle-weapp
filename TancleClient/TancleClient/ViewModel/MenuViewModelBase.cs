using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleClient.ViewModel.Interface;

namespace TancleClient.ViewModel
{
    public class MenuViewModelBase : ViewModelBase, IViewModelUpdateDisplayList, IDisplayLanguageChange
    {
        protected string Title = "";


        //private string _title;

        ///// <summary>
        ///// View's display name
        ///// </summary>
        //public string Title
        //{
        //    get { return _title; }
        //    set
        //    {
        //        _title = value;
        //        RaisePropertyChanged("Title");
        //    }
        //}

        private string _icon;
        /// <summary>
        /// View's display name
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                this.RaisePropertyChanged("Icon");
            }
        }

        protected string SearchHintText = "";

        public virtual void DisplayTitle()
        {
            throw new NotImplementedException($"{GetType().Name} does NOT implement DisplayTitle() yet!");
        }

        public virtual void UpdateDataList(int pageIndex, out int total, int itemPerPage = 10)
        {
            throw new NotImplementedException($"{GetType().Name} does NOT implement UpdateDisplayList() yet!");
        }

        public virtual bool QueryDatabaseFailure()
        {
            return false;
        }

        public virtual void DisplayLanguageChange()
        {
            return;
        }
    }
}
