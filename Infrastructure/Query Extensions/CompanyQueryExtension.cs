using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Query_Extensions
{
    public static class CompanyQueryExtension
    {
        public static IQueryable<Company> Search(this IQueryable<Company> companies, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))

                return companies;
            var serachTermToLowerCase = searchTerm.ToLower();

            var value = companies.Where(e => e.Name.ToLower().Contains(serachTermToLowerCase));

            return value;
        }
    }
}
