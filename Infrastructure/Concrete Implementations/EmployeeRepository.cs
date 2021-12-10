using Core.Models;
using Infrastructure.Abstractions;
using Infrastructure.Data_Transfer_Objects;
using Infrastructure.Database_Context;
using Infrastructure.Employee_Repository_Extensions;
using Infrastructure.Query_Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<PagedList<Employee>> GetAllEmployeesOfACompany(Guid companyId, EmployeeParameter employeeParameter, bool trackChanges)
        {
            var employees = await FindByCondition(e => e.CompanyId
            .Equals(companyId) && e.Age >= employeeParameter.MinAge && e.Age <= employeeParameter.MaxAge, trackChanges)
            .Search(employeeParameter.SearchTerm)
            .OrderBy(c => c.Name)
            .ToListAsync();

            return PagedList<Employee>.ToPagedList(employees, employeeParameter.pageNumber, employeeParameter.pageSize);
        }

        public async Task<Employee> GetAnEmployeeFromACompany(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var employee = FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).SingleOrDefaultAsync();

            return await employee;
        }

        public async Task<IEnumerable<Employee>> GetMultipleEmployeesById(Guid companyId, IEnumerable<Guid> employeeIds, bool trackChanges)
        {
            var value = FindByCondition(e => employeeIds.Contains(e.Id), trackChanges).ToListAsync();

            return await value;
        }
    }
}
