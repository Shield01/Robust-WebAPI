using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Abstractions
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> FindAllCompanies(bool trackChanges);

        Company FindCompany(Guid id, bool trackChanges);

        void CreateCompany(Company company);

        void CreateMultipleCompanies(IEnumerable<Company> companies);

        IEnumerable<Company> FindMultipleCompanies(IEnumerable<Guid> guids, bool trackChanges);
    }
}
