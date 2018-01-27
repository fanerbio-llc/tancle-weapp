using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text;
using TancleDataModel.Interface;

namespace TancleDataModel.Model
{
    public class Sickness: IDataValidatable, IOperationRecordEntity
    {
        public ValidationResults Validate()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new FileConfigurationSource("Validation/SicknessValidation.config"));

            List<string> ruleSetList = new List<string>();
            ruleSetList.Add("Mandatory");

            string[] ruleSet = ruleSetList.ToArray();
            return Validation.Validate(this, ruleSet);
        }



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(200)]
        public string SicknessNo { get; set; }

        [MaxLength(200)]
        public string SicknessName { get; set; }

        [MaxLength(50)]
        public DateTime CreatedTime { get; set; }

        [MaxLength(200)]
        public DateTime UpdatedTime { get; set; }


        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            //buffer.AppendLine($"{Resources.Site_HospitalName}: {HospitalName}");
            //buffer.AppendLine($"{Resources.Site_HospitalAddress}: {HospitalAddress}");
            //buffer.AppendLine($"{Resources.Site_Phone}: {Phone}");
            //buffer.AppendLine($"{Resources.Site_Manufacture}: {Manufacture}");
            //buffer.AppendLine($"{Resources.Site_ModelName}: {ModelName}");
            //buffer.AppendLine($"{Resources.Site_SeriesNumber}: {SeriesNumber}");
            //buffer.AppendLine($"{Resources.Site_SoftwareVersion}: {SoftwareVersion}");
            //buffer.AppendLine($"{Resources.Site_FirmwareVersion}: {FirmwareVersion}");

            return buffer.ToString();
        }

        public string RecordActionName()
        {
            return "View_ActionName_SicknessManagement";
        }

        public object RecordEntity(DbContext context)
        {
            return null;
        }
    }
}
