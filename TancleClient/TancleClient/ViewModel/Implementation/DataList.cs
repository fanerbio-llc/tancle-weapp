using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TancleClient.ViewModel.Interface;
using TancleDataModel;
using TancleDataModel.Implementation;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace TancleClient.ViewModel.Implementation
{
    public class DataList<TEntity> : ViewModelBase, IDataList<TEntity>
         where TEntity : class
    {
        protected static readonly DataAccessServiceGeneric<TancleConfigDbContext, TEntity> DataService =
            ServiceLocator.Current.GetInstance<DataAccessServiceGeneric<TancleConfigDbContext, TEntity>>();

        private ObservableCollection<DataListItem> _items;

        public ObservableCollection<DataListItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                this.RaisePropertyChanged("Items");
            }
        }

        public DataList()
        {
            _items = new ObservableCollection<DataListItem>();

            ResetItems();
        }

        public virtual void ResetItems()
        {
            throw new NotImplementedException($"{GetType().Name} does NOT implement ResetDataList() yet!");
        }

        public virtual void SetCheckedItems(List<TEntity> items)
        {
            throw new NotImplementedException($"{GetType().Name} does NOT implement SetCheckedItems() yet!");
        }

        public virtual int[] GetCheckedItems()
        {
            var checkItems = new List<int>();

            Items.ForEach(x =>
            {
                if (x.IsChecked)
                {
                    checkItems.Add(x.Id);
                }
            });

            return checkItems.ToArray();
        }
    }
}
