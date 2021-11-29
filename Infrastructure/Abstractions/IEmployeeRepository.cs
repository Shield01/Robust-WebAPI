﻿using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Abstractions
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployeesOfACompany(Guid companyId, bool trackChanges);

        void CreateEmployee(Guid companyId, Employee employee);

        Employee GetAnEmployeeFromACompany(Guid companyId, Guid employeeId, bool trackChanges);

        void DeleteEmployee(Employee employee);
    }
}