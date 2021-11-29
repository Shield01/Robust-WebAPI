using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class BaseModel
    {
        [Required(ErrorMessage = "Date created is not being recorded")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Date created is not being recorded")]
        public DateTime DateModified { get; set; }

        public bool IsEnabled { get; set; }
    }
}
