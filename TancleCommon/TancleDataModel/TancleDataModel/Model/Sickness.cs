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
    public class Sickness: IDataValidatable, IOperationRecordEntity, ICloneable, ICopyable<Sickness>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(2), MaxLength(50)]
        public string SicknessNo { get; set; }

        [Required]
        [MinLength(2), MaxLength(200)]
        public string SicknessName { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public virtual ICollection<Habit> Habits { get; set; }

        public virtual ICollection<Area> Areas { get; set; }

        public virtual ICollection<Advice> Advice { get; set; }

        public object Clone()
        {
            Sickness newItem = new Sickness();
            CopyTo(newItem);
            return newItem;
        }

        public void CopyTo(Sickness destination)
        {
            destination.Id = Id;
            destination.SicknessNo = SicknessNo;
            destination.SicknessName = SicknessName;
            destination.CreatedTime = CreatedTime;
            destination.UpdatedTime = UpdatedTime;
        }

        public ValidationResults Validate()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new FileConfigurationSource("Validation/SicknessValidation.config"));

            List<string> ruleSetList = new List<string>();
            ruleSetList.Add("Mandatory");

            string[] ruleSet = ruleSetList.ToArray();
            return Validation.Validate(this, ruleSet);
        }

        public string ToString(string placeHolder)
        {
            return string.Format(placeHolder, SicknessNo, SicknessName, CreatedTime, UpdatedTime);
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
