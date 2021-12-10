using Core.Models;
using Infrastructure.Query_Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Employee_Repository_Extensions
{
    public static class EmployeeQueryExtension
    {
        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            //{
                return employees;

                var searchTermToLowerCase = searchTerm.Trim().ToLower();

                var value = employees.Where(e => e.Name.ToLower().Contains(searchTermToLowerCase));

                return value;
            //}
        }
    }
}
