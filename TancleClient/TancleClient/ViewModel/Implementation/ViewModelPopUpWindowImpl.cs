using Microsoft.Practices.ServiceLocation;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TancleClient.Service;
using TancleClient.Utility;
using TancleClient.ViewModel.Interface;

namespace TancleClient.ViewModel.Implementation
{
    public class ViewModelPopUpWindowImpl : IViewModelConfirmWindow, IViewModelHintWindow, IViewModelErrorWindow
    {
        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        /// <summary>
        /// Pop up confirm window
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="confirmText"></param>
        public bool ConfirmWindow(INotifyPropertyChanged vm, string confirmText)
        {
            // Pop up dialog to require confirmation

            MessageBoxManager.Yes = TranslationService.Translate("View_Messagebox_Button_Yes")?.ToString();
            MessageBoxManager.No = TranslationService.Translate("View_Messagebox_Button_No")?.ToString();
            MessageBoxManager.Register();

            var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();

            MessageBoxResult rsl = dialogService.ShowMessageBox(vm,
                confirmText,
                TranslationService.Translate("View_Application_Title")?.ToString(),
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            MessageBoxManager.Unregister();

            if (rsl.ToString().Equals("No"))
            {
                // cancel
                return false;
            }

            return true;
        }

        /// <summary>
        /// Pop up hint window
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="hintText"></param>
        public void HintWindow(INotifyPropertyChanged vm, string hintText)
        {
            MessageBoxManager.OK = TranslationService.Translate("View_Messagebox_Button_Ok")?.ToString();
            MessageBoxManager.Register();

            var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
            MessageBoxResult rsl = dialogService.ShowMessageBox(vm,
                hintText,
                TranslationService.Translate("View_Application_Title")?.ToString(),
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            MessageBoxManager.Unregister();
        }

        /// <summary>
        /// Pop up error window
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="errorText"></param>
        public void ErrorWindow(INotifyPropertyChanged vm, string errorText)
        {
            MessageBoxManager.OK = TranslationService.Translate("View_Messagebox_Button_Ok").ToString();
            MessageBoxManager.Register();

            var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
            MessageBoxResult rsl = dialogService.ShowMessageBox(
                vm,
                errorText,
                TranslationService.Translate("View_Application_Title")?.ToString(),
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            MessageBoxManager.Unregister();
        }
    }
}
