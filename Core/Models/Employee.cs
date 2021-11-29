using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Employee : BaseModel
    {
        [Column("Employee Id")]
        [Required(ErrorMessage = "Id is a required field")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is a required field")]
        [MaxLength(60, ErrorMessage = "Maximum amount of charcters accepted is 30")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is a required field")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum amount of characters accepted is 30")]
        public string Position { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
