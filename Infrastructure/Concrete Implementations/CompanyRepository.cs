using Core.Models;
using Infrastructure.Abstractions;
using Infrastructure.Database_Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Concrete_Implementations
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(InfrastructureDbContext dbContext) : base (dbContext)
        {

        }

        public void CreateCompany(Company company)
        {
            company.DateCreated = DateTime.Now;

            company.IsEnabled = true;

            Create(company);
        }

        public void CreateMultipleCompanies(IEnumerable<Company> companies)
        {
            foreach (var company in companies)
            {
                company.DateCreated = DateTime.Now;

                company.IsEnabled = true;

                Create(company);
            }
        }

        public void DeleteCompany(Company company)
        {
            Delete(company);
        }

        public async Task<IEnumerable<Company>> FindAllCompanies(bool trackChanges) => 
            await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<Company> FindCompany(Guid id, bool trackChanges)
        {
            var value = FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

            return await value;
        }

        public async Task<IEnumerable<Company>> FindMultipleCompanies(IEnumerable<Guid> guids, bool trackChanges)
        {
            var value = FindByCondition(e => guids.Contains(e.Id), trackChanges).ToListAsync();

            return await value;
        }
    }
}
