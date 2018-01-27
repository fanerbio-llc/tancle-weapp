using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TancleDataModel.Model
{
    public class Habit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(200)]
        public string HabitNo { get; set; }

        [MaxLength(200)]
        public string HabitName { get; set; }

        [MaxLength(50)]
        public DateTime CreatedTime { get; set; }

        [MaxLength(200)]
        public DateTime UpdatedTime { get; set; }
    }
}
