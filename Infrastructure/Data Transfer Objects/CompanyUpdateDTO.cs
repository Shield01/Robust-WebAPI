using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.Data_Transfer_Objects
{
    public class CompanyUpdateDTO
    {
        [MaxLength(60, ErrorMessage = "The Maximum amount of characters accepted for the name field is 60 characters, kindly ensure your name does not exceed this amount of characters.")]
        [Required(ErrorMessage = "The name field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You haven't provided your address info")]
        [MaxLength(60, ErrorMessage = "The Maximum amount of characters accepted for the address field is 60 characters, kindly ensure your address does not exceed this amount of characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Your country's name is required")]
        [MaxLength(30, ErrorMessage = "The country name is too long")]
        public string Country { get; set; }

        public IEnumerable<EmployeeInputDTO> Employees { get; set; }
    }
}
