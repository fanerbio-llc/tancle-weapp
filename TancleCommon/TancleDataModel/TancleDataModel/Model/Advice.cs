using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TancleDataModel.Model
{
    public class Advice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(200)]
        public string AdviceNo { get; set; }

        [MaxLength(200)]
        public string AdviceName { get; set; }

        [MaxLength(50)]
        public DateTime CreatedTime { get; set; }

        [MaxLength(200)]
        public DateTime UpdatedTime { get; set; }
    }
}
