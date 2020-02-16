using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dogs2.Models
{
    public class QueueModel 
    {

        [Key]
        public int queueId { get; set; }
        
        public int userId { get; set; }

        [Display(Name = "Insert Date")]
        public DateTime insertDateTime { get; set; }

        [Required(ErrorMessage = "You must enter a value for Queue DateTime field!")]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime queueDateTime { get; set; }

        [ForeignKey("userId")]
        public UsersModel users1 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (queueDateTime < DateTime.Now)
            {
                yield return new ValidationResult(
                    $"Queue date must be greate than now",
                    new[] { nameof(userId) });
            }
        }
    }

}
