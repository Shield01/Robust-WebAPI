using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data_Transfer_Objects
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FullAddress { get; set; }

        public IEnumerable<EmployeeDTO> Employees { get; set; }
    }
}
