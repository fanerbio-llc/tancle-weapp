 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
 using TancleClient.ViewModel.Interface;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace TancleClient.ViewModel
{
    /// <summary>
    /// Implementation of paginator
    /// </summary>
    public class PaginatorViewModel : ViewModelBase, IViewModelPaginator
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Private fields

        private int _currentPageIndex;
        private int _totalPage;
        private int _itemPerPage;
        private int _total;

        private bool _firstBtnEnable;
        private bool _prevBtnEnable;
        private bool _nextBtnEnable;
        private bool _lastBtnEnable;
        #endregion

        #region Properties bind to view

        /// <summary>
        /// Record total page. Bind to "TotalPage" TextBlock of view.
        /// </summary>
        public int TotalPage
        {
            get { return _totalPage; }
            set
            {
                _totalPage = value;
                this.RaisePropertyChanged("TotalPage");
            }
        }

        /// <summary>
        /// Record current page index. Bind to "CurrentPage" TextBlock of view.
        /// </summary>
        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set
            {
                _currentPageIndex = value;
                this.RaisePropertyChanged("CurrentPageIndex");
            }
        }

        /// <summary>
        /// Items per page. Bind to "ItemPerPage" TextBlock of view.
        /// </summary>
        public int ItemPerPage
        {
            get { return _itemPerPage; }
            set
            {
                _itemPerPage = value;
                this.RaisePropertyChanged("ItemPerPage");
            }
        }

        /// <summary>
        /// Record total item. Bind to "Total" TextBlock of view.
        /// </summary>
        public int Total
        {
            get { return _total; }
            set
            {
                _total = value;
                this.RaisePropertyChanged("Total");
            }
        }

        public bool FirstBtnEnable
        {
            get { return _firstBtnEnable; }
            set
            {
                _firstBtnEnable = value;
                this.RaisePropertyChanged("FirstBtnEnable");
            }
        }

        public bool PrevBtnEnable
        {
            get { return _prevBtnEnable; }
            set
            {
                _prevBtnEnable = value;
                this.RaisePropertyChanged("PrevBtnEnable");
            }
        }

        public bool NextBtnEnable
        {
            get { return _nextBtnEnable; }
            set
            {
                _nextBtnEnable = value;
                this.RaisePropertyChanged("NextBtnEnable");
            }
        }

        public bool LastBtnEnable
        {
            get { return _lastBtnEnable; }
            set
            {
                _lastBtnEnable = value;
                this.RaisePropertyChanged("LastBtnEnable");
            }
        }
        #endregion

        #region Properties

        public const int FirstPageNumber = 1;

        #endregion

        #region Constructors

        public PaginatorViewModel()
        {
            CurrentPageIndex = FirstPageNumber;
            TotalPage = 0;
            ItemPerPage = 10;
            Total = 0;

            // Below line will cause UpdateDisplayList call twice, one is ViewSelectedChange in MainViewModel, really need it?
            //UpdateDisplayList(ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView());

            FirstCommand = new RelayCommand(BtnFirst_Click);
            PrevCommand = new RelayCommand(BtnPrev_Click);
            NextCommand = new RelayCommand(BtnNext_Click);
            LastCommand = new RelayCommand(BtnLast_Click);
        }

        #endregion

        #region Command definitions

        /// <summary>
        /// Bind to "BtnFirst" button of view
        /// </summary>
        public ICommand FirstCommand { get; private set; }
        /// <summary>
        /// Bind to "BtnPrev" button of view
        /// </summary>
        public ICommand PrevCommand { get; private set; }
        /// <summary>
        /// Bind to "BtnNext" button of view
        /// </summary>
        public ICommand NextCommand { get; private set; }
        /// <summary>
        /// Bind to "BtnLast" button of view
        /// </summary>
        public ICommand LastCommand { get; private set; }
        #endregion

        #region Commmand implementations

        /// <summary>
        /// "First Page" button is clicked, update display list.
        /// </summary>
        private void BtnFirst_Click()
        {
            // Display the first page

            CurrentPageIndex = FirstPageNumber;
            UpdateDisplayList(ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView());
        }
        /// <summary>
        /// "Previous Page" button is clicked, update display list.
        /// </summary>
        private void BtnPrev_Click()
        {
            // Display previous page

            if (CurrentPageIndex > FirstPageNumber)
            {
                CurrentPageIndex--;
                UpdateDisplayList(ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView());
            }
        }
        /// <summary>
        /// "Next Page" button is clicked, update display list.
        /// </summary>
        private void BtnNext_Click()
        {
            // Display next page

            if (CurrentPageIndex < TotalPage)
            {
                CurrentPageIndex++;
                UpdateDisplayList(ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView());
            }
        }
        /// <summary>
        /// "Last Page" button is clicked, update display list.
        /// </summary>
        private void BtnLast_Click()
        {
            // Display the last page

            if (CurrentPageIndex != TotalPage)
            {
                CurrentPageIndex = TotalPage;
                UpdateDisplayList(ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView());
            }
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Reset to display content of the #1 page
        /// </summary>
        public void ResetDisplayPage()
        {
            CurrentPageIndex = FirstPageNumber;
            TotalPage = 0;
            Total = 0;
            UpdateDisplayList(ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView());
        }

        /// <summary>
        /// Update display content
        /// </summary>
        public void UpdateDisplayList(IViewModelUpdateDisplayList updateDisplayListImpl)
        {
            // Prevnet CurrentPageIndex from 0.

            CurrentPageIndex = CurrentPageIndex == 0 ? FirstPageNumber : CurrentPageIndex;

            // Get display content

            int total = 0;
            try
            {
                updateDisplayListImpl.UpdateDataList(CurrentPageIndex, out total, ItemPerPage);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                total = 0;
            }

            // update total item
            Total = total;

            if (total == 0)
            {
                // there is no content

                TotalPage = 0;
                //CurrentPageIndex = FirstPageNumber;
                CurrentPageIndex = TotalPage;
            }
            else
            {
                // Calculate the total pages

                TotalPage = total / ItemPerPage;
                if (total % ItemPerPage != 0)
                {
                    TotalPage += 1;
                }

                // check whether the last one is deleted.

                if (CurrentPageIndex > TotalPage)
                {
                    // if yes, update CurrentPageIndex, get last page again

                    CurrentPageIndex = TotalPage;
                    try
                    {
                        updateDisplayListImpl.UpdateDataList(CurrentPageIndex, out total, ItemPerPage);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message, e);
                    }
                }
            }

            FirstBtnEnable = CurrentPageIndex > FirstPageNumber;
            LastBtnEnable = CurrentPageIndex < TotalPage;
            PrevBtnEnable = FirstBtnEnable && CurrentPageIndex != FirstPageNumber;
            NextBtnEnable = LastBtnEnable && CurrentPageIndex != TotalPage;
        }

        #endregion

    }
}
