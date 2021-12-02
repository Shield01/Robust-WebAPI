using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesOfACompany(Guid companyId, bool trackChanges);

        void CreateEmployee(Guid companyId, Employee employee);

        Task<Employee> GetAnEmployeeFromACompany(Guid companyId, Guid employeeId, bool trackChanges);

        void DeleteEmployee(Employee employee);

        Task<IEnumerable<Employee>> GetMultipleEmployeesById(Guid companyId, IEnumerable<Guid> employeeIds, bool trackChanges);

        void CreateMultipleEmployee(Guid companyId, IEnumerable<Employee> employees);
    }
}
