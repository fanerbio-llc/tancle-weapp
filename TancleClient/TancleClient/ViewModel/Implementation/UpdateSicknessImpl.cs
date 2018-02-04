using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleClient.Service;
using TancleClient.ViewModel.Interface;
using TancleDataModel;
using TancleDataModel.IDataAccessService;
using TancleDataModel.Implementation;
using TancleDataModel.Model;

namespace TancleClient.ViewModel.Implementation
{
    public class UpdateSicknessImpl: IViewModelUpdateSickness
    {
        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        public bool AddSickness(Sickness sickness, string popUpText, bool popUpConfirm)
        {
            var vm = ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView();
            var dataService = ServiceLocator.Current.GetInstance<DataAccessServiceGeneric<TancleConfigDbContext, Sickness>>();

            if (popUpConfirm)
            {
                string hintText = TranslationService.Translate("View_Messagebox_ConfirmToAdd") + ": \n" + popUpText;

                if (ServiceLocator.Current.GetInstance<IViewModelConfirmWindow>().ConfirmWindow(vm, hintText) == false)
                {
                    return false;
                }
            }

            var rc = false;

            var result = dataService.Add(sickness);

            if (result.ResultCode != ResultCodeOption.Ok)
            {
                string hintText;
                if (result.ResultCode == ResultCodeOption.Duplicate)
                {
                    hintText =
                        TranslationService.Translate("View_Messagebox_CannotAdd") + ": \n" +
                        popUpText + "\n" +
                        TranslationService.Translate("View_Messagebox_Reason") + ": " +
                        TranslationService.Translate("View_Messagebox_ExistAlready");
                }
                else
                {
                    hintText =
                        TranslationService.Translate("View_Messagebox_CannotAdd") + ": \n" +
                        popUpText + "\n" +
                        TranslationService.Translate("View_Messagebox_Reason") + ": " + result.Message;
                }

                ServiceLocator.Current.GetInstance<IViewModelErrorWindow>().ErrorWindow(vm, hintText);
                rc = false;
            }
            else
            {
                var habitList = ServiceLocator.Current.GetInstance<HabitSicknessViewModel>().GetCheckedItems();
                var adviceList = ServiceLocator.Current.GetInstance<AdviceSicknessViewModel>().GetCheckedItems();
                var areaList = ServiceLocator.Current.GetInstance<AreaSicknessViewModel>().GetCheckedItems();

                result = ServiceLocator.Current.GetInstance<ISicknessService>().ModifySickness(sickness, habitList, adviceList, areaList);

                if (result.ResultCode == ResultCodeOption.Ok)
                {
                    ServiceLocator.Current.GetInstance<IViewModelHintWindow>().HintWindow(
                       vm,
                       TranslationService.Translate("View_Messagebox_Success") + "!");

                    rc = true;
                }
                else
                {
                    ServiceLocator.Current.GetInstance<IViewModelErrorWindow>().ErrorWindow(
                        vm,
                        TranslationService.Translate("View_Messagebox_Failure") +": \n" + 
                        TranslationService.Translate("View_Messagebox_Reason") + ": " + result.Message);

                    rc = false;
                }
            }

            return rc;
        }

        public bool UpdateSickness(Sickness sickness, string popUpText, bool popUpConfirm)
        {
            var vm = ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView();
            var dataService = ServiceLocator.Current.GetInstance<DataAccessServiceGeneric<TancleConfigDbContext, Sickness>>();

            if (popUpConfirm)
            {
                string hintText = TranslationService.Translate("View_Messagebox_ConfirmToUpdate") + ": \n" + popUpText;

                if (ServiceLocator.Current.GetInstance<IViewModelConfirmWindow>().ConfirmWindow(vm, hintText) == false)
                {
                    return false;
                }
            }

            var rc = false;

            var tuple = dataService.LoadSingleTuple(sickness.Id);
            if (tuple == null)
            {
                string hintText = popUpText + "\n" + TranslationService.Translate("View_Messagebox_DeleteAlready");
            }
            else
            {
                sickness.CopyTo(tuple);

                var result = dataService.Modify(tuple);

                if (result.ResultCode != ResultCodeOption.Ok)
                {
                    string hintText =
                         TranslationService.Translate("View_Messagebox_CannotUpdate") + ": \n" +
                         popUpText + "\n" +
                         TranslationService.Translate("View_Messagebox_Reason") + ": " + result.Message;

                    ServiceLocator.Current.GetInstance<IViewModelErrorWindow>().ErrorWindow(vm, hintText);
                    rc = false;
                }
                else
                {
                    var habitList = ServiceLocator.Current.GetInstance<HabitSicknessViewModel>().GetCheckedItems();
                    var adviceList = ServiceLocator.Current.GetInstance<AdviceSicknessViewModel>().GetCheckedItems();
                    var areaList = ServiceLocator.Current.GetInstance<AreaSicknessViewModel>().GetCheckedItems();

                    result = ServiceLocator.Current.GetInstance<ISicknessService>().ModifySickness(tuple, habitList, adviceList, areaList);

                    if (result.ResultCode == ResultCodeOption.Ok)
                    {
                        ServiceLocator.Current.GetInstance<IViewModelHintWindow>().HintWindow(
                           vm,
                           TranslationService.Translate("View_Messagebox_Success") + "!");

                        rc = true;
                    }
                    else
                    {
                        ServiceLocator.Current.GetInstance<IViewModelErrorWindow>().ErrorWindow(
                            vm,
                            TranslationService.Translate("View_Messagebox_Failure") + "\n" +
                            TranslationService.Translate("View_Messagebox_Reason") + ": " + result.Message);

                        rc = false;
                    }
                }
            }

            return rc;
        }
    }
}
