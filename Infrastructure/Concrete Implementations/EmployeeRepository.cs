using Core.Models;
using Infrastructure.Abstractions;
using Infrastructure.Data_Transfer_Objects;
using Infrastructure.Database_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Concrete_Implementations
{
    public class EmployeeRepository : BaseRepository<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(InfrastructureDbContext dbContext) : base(dbContext)
        {

        }

        public void CreateEmployee(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;

            employee.IsEnabled = true;

            employee.DateCreated = DateTime.Now;

            Create(employee);
        }

        public void CreateMultipleEmployee(Guid companyId, IEnumerable<Employee> employees)
        {
            foreach(var employee in employees)
            {
                employee.CompanyId = companyId;

                employee.DateCreated = DateTime.Now;

                employee.IsEnabled = true;

                Create(employee);
            }
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }

        public IEnumerable<Employee> GetAllEmployeesOfACompany(Guid companyId, bool trackChanges)
        {
            var value = FindByCondition(e => e.CompanyId
            .Equals(companyId), trackChanges).OrderBy(c => c.Name);

            return value;
        }

        public Employee GetAnEmployeeFromACompany(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var employee = FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).SingleOrDefault();

            return employee;
        }

        public IEnumerable<Employee> GetMultipleEmployeesById(Guid companyId, IEnumerable<Guid> employeeIds, bool trackChanges)
        {
            var value = FindByCondition(e => employeeIds.Contains(e.Id), trackChanges).ToList();

            return value;
        }
    }
}
