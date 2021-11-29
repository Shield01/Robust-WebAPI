using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models
{
    public class Company : BaseModel
    {
        [Column("Company Id")]
        [Required(ErrorMessage = "Id is a required field")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum amount of characters accepted is 30")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is a required field")]
        [MaxLength(60, ErrorMessage = "Maximum amount of characters accepted is 60")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is a required field")]
        public string Country { get; set; }

        public IEnumerable<Employee> Employees { get; set; }
    }
}
