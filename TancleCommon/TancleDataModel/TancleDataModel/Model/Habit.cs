﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using TancleDataModel.Interface;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace TancleDataModel.Model
{
    public class Habit : ICloneable, ICopyable<Habit>, IDataValidatable, IOperationRecordEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(2), MaxLength(50)]
        public string HabitNo { get; set; }

        [Required]
        [MinLength(2), MaxLength(200)]
        public string HabitName { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public virtual ICollection<Sickness> Sicknesses { get; set; }

        public object Clone()
        {
            Habit newItem = new Habit();
            CopyTo(newItem);
            return newItem;
        }

        public void CopyTo(Habit destination)
        {
            destination.Id = Id;
            destination.HabitNo = HabitNo;
            destination.HabitName= HabitName;
            destination.CreatedTime= CreatedTime;
            destination.UpdatedTime= UpdatedTime;
        }

        public ValidationResults Validate()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new FileConfigurationSource("Validation/HabitValidation.config"));

            List<string> ruleSetList = new List<string>();
            ruleSetList.Add("Mandatory");

            string[] ruleSet = ruleSetList.ToArray();
            return Validation.Validate(this, ruleSet);
        }

        public string ToString(string placeHolder)
        {
            return string.Format(placeHolder, HabitNo, HabitName, CreatedTime, UpdatedTime);
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
