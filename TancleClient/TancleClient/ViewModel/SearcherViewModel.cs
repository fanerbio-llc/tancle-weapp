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
    /// Implementation of searcher
    /// </summary>
    public class SearcherViewModel : ViewModelBase, IViewModelSearcher
    {
        #region Private fields

        private string _searchText;

        #endregion

        #region Properties bind to view

        /// <summary>
        /// Bind to search Textbox of view
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                this.RaisePropertyChanged("SearchText");
            }
        }

        #endregion


        #region Properties

        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors

        public SearcherViewModel()
        {
            ResetSearchText();
            SearchCommand = new RelayCommand(BtnSearch_Click);
            GotFocusCommand = new RelayCommand(GotFocus);
            LostFocusCommand = new RelayCommand(LostFocus);
        }

        #endregion

        #region Command definitions

        /// <summary>
        /// Bind to "BtnSearch" button of view
        /// </summary>
        public ICommand SearchCommand { get; private set; }
        /// <summary>
        /// Bind to search textbox got focus event of view
        /// </summary>
        public ICommand GotFocusCommand { get; private set; }
        /// <summary>
        /// Bind to search textbox lost focus event of view
        /// </summary>
        public ICommand LostFocusCommand { get; private set; }

        #endregion

        #region Commmand implementations

        /// <summary>
        /// Search button is clicked, update display list.
        /// </summary>
        private void BtnSearch_Click()
        {
            var paginator = ServiceLocator.Current.GetInstance<IViewModelPaginator>();
            paginator.ResetDisplayPage();
        }

        /// <summary>
        /// Set SearchText to empty.
        /// </summary>
        private void GotFocus()
        {
            SearchText = "";
        }

        /// <summary>
        /// Restore default SearchText
        /// </summary>
        private void LostFocus()
        {
            if (string.IsNullOrEmpty(SearchText))
                SearchText = ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView().SearchHintText;
        }
        #endregion

        #region Public functions

        /// <summary>
        /// Get SearchText
        /// </summary>
        /// <returns></returns>
        public string GetSearchText()
        {
            return SearchText;
        }

        /// <summary>
        /// Set SearchText to default hint text.
        /// </summary>
        public void ResetSearchText()
        {
            SearchText = ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView().SearchHintText;
        }

        /// <summary>
        /// Check whether search text is input
        /// </summary>
        /// <returns></returns>
        public bool Search()
        {
            // return true when both SearchText is not empty and SearchText is not default hint text.

            return !string.IsNullOrEmpty(SearchText) && 
                !SearchText.Equals(ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView().SearchHintText);
        }

        #endregion
    }
}
