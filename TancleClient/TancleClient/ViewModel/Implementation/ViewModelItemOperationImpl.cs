using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleClient.Service;
using TancleClient.ViewModel.Interface;
using System.Data.Entity;
using TancleDataModel.Implementation;
using TancleDataModel.Interface;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace TancleClient.ViewModel.Implementation
{
    public class ViewModelItemOperationImpl: IViewModelAddItem, IViewModelUpdateItem, IViewModelDeleteItem, IViewModelValidateData
    {
        private static readonly ITranslationService TranslationService = ServiceLocator.Current.GetInstance<ITranslationService>();

        public bool Add<TDbContext, TEntity>(BaseDataAccessService<TDbContext, TEntity> dataService, TEntity entity, string popUpText, bool popUpConfirm)
            where TDbContext : DbContext
            where TEntity : class
        {
            var vm = ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView();

            if (popUpConfirm)
            {
                string hintText = TranslationService.Translate("View_Messagebox_ConfirmToAdd") + ": \n" + popUpText;

                if (ServiceLocator.Current.GetInstance<IViewModelConfirmWindow>().ConfirmWindow(vm, hintText) == false)
                {
                    return false;
                }
            }

            var rc = false;

            var result = dataService.Add(entity);

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
                ServiceLocator.Current.GetInstance<IViewModelHintWindow>().HintWindow(
                    vm,
                    TranslationService.Translate("View_Messagebox_Success") + "!");

                rc = true;
            }

            return rc;
        }

        public bool Update<TDbContext, TEntity>(BaseDataAccessService<TDbContext, TEntity> dataService, int entityId, ICopyable<TEntity> copyImpl, string popUpText, bool popUpConfirm)
            where TDbContext : DbContext
            where TEntity : class
        {
            var vm = ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView();

            if (popUpConfirm)
            {
                string hintText = TranslationService.Translate("View_Messagebox_ConfirmToUpdate") + ": \n" + popUpText;

                if (ServiceLocator.Current.GetInstance<IViewModelConfirmWindow>().ConfirmWindow(vm, hintText) == false)
                {
                    return false;
                }
            }

            var rc = false;

            var tuple = dataService.LoadSingleTuple(entityId);
            if (tuple == null)
            {
                string hintText = popUpText + "\n" + TranslationService.Translate("View_Messagebox_DeleteAlready");
            }
            else {
                copyImpl.CopyTo(tuple);

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
                else {
                    ServiceLocator.Current.GetInstance<IViewModelHintWindow>().HintWindow(
                        vm,
                        TranslationService.Translate("View_Messagebox_Success") + "!");
                    rc = true;
                }
            }

            return rc;
        }

        public bool Delete<TDbContext, TEntity>(BaseDataAccessService<TDbContext, TEntity> dataService, int entityId, string popUpText, bool popUpConfirm)
            where TDbContext : DbContext
            where TEntity : class
        {
            var vm = ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView();

            if (popUpConfirm)
            {
                string hintText = TranslationService.Translate("View_Messagebox_ConfirmToDelete") + ": \n" + popUpText;

                if (ServiceLocator.Current.GetInstance<IViewModelConfirmWindow>().ConfirmWindow(vm, hintText) == false)
                {
                    return false;
                }
            }

            var rc = false;

            var result = dataService.Delete(entityId);

            if (result.ResultCode != ResultCodeOption.Ok)
            {
                string hintText =
                        TranslationService.Translate("View_Messagebox_ConfirmToDelete") + ": \n" +
                        popUpText + "\n" +
                        TranslationService.Translate("View_Messagebox_Reason") + ": " + result.Message;

                ServiceLocator.Current.GetInstance<IViewModelErrorWindow>().ErrorWindow(vm, hintText);
                rc = false;
            }
            else
            {
                ServiceLocator.Current.GetInstance<IViewModelHintWindow>().HintWindow(
                    vm,
                    TranslationService.Translate("View_Messagebox_Success") + "!");

                rc = true;
            }

            return rc;
        }

        public bool Validate(IDataValidatable validator)
        {
            ValidationResults vr = validator.Validate();

            if (vr.IsValid)
            {
                return true;
            }

            string errorStr = "";
            for (int i = 0; i < vr.Count; i++)
            {
                errorStr += TranslationService.Translate(vr.ElementAt(i).Message) + "\n";
            }

            var vm = ServiceLocator.Current.GetInstance<IViewModelGetSelectedView>().GetSelectedView();

            ServiceLocator.Current.GetInstance<IViewModelErrorWindow>().ErrorWindow(vm, errorStr);

            return false;
        }
    }
}
