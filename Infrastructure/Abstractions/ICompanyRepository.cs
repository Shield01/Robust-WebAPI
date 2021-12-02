using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> FindAllCompanies(bool trackChanges);

        Task<Company> FindCompany(Guid id, bool trackChanges);

        void CreateCompany(Company company);

        void CreateMultipleCompanies(IEnumerable<Company> companies);

        Task<IEnumerable<Company>> FindMultipleCompanies(IEnumerable<Guid> guids, bool trackChanges);

        void DeleteCompany(Company company);
    }
}
