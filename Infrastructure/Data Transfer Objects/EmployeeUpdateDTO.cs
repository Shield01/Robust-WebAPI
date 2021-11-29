using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.Data_Transfer_Objects
{
    public class EmployeeUpdateDTO
    {
        [Required(ErrorMessage = "The name field is required")]
        [MaxLength(60, ErrorMessage = "The Maximum amount of characters accepted for the name field is 60 characters, kindly ensure your name does not exceed this amount of characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The age field is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "The position field is required")]
        [MaxLength(30, ErrorMessage = "The Maximum amount of characters accepted for the position field is 30 characters, kindly ensure your position does not exceed this amount of characters.")]
        public string Position { get; set; }
    }
}
