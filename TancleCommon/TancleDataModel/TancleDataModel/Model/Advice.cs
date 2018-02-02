using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using TancleDataModel.Interface;

namespace TancleDataModel.Model
{
    public class Advice : ICloneable, ICopyable<Advice>, IDataValidatable, IOperationRecordEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(2), MaxLength(50)]
        public string AdviceNo { get; set; }

        [Required]
        [MinLength(2), MaxLength(200)]
        public string AdviceName { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public virtual ICollection<Sickness> Sicknesses { get; set; }

        public object Clone()
        {
            Advice newItem = new Advice();
            CopyTo(newItem);
            return newItem;
        }

        public void CopyTo(Advice destination)
        {
            destination.Id = Id;
            destination.AdviceNo = AdviceNo;
            destination.AdviceName = AdviceName;
            destination.CreatedTime = CreatedTime;
            destination.UpdatedTime = UpdatedTime;
        }

        public ValidationResults Validate()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new FileConfigurationSource("Validation/AdviceValidation.config"));

            List<string> ruleSetList = new List<string>();
            ruleSetList.Add("Mandatory");

            string[] ruleSet = ruleSetList.ToArray();
            return Validation.Validate(this, ruleSet);
        }

        public string ToString(string placeHolder)
        {
            return string.Format(placeHolder, AdviceNo, AdviceName, CreatedTime, UpdatedTime);
        }

        public string RecordActionName()
        {
            throw new NotImplementedException();
        }

        public object RecordEntity(DbContext context)
        {
            throw new NotImplementedException();
        }
    }
}
