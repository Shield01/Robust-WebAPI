﻿using Infrastructure.Abstractions;
using Infrastructure.Concrete_Implementations;
using Infrastructure.Database_Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository_Manager
{
    public class RepositoryManager : IRepositoryManager
    {
        private EmployeeRepository employeeRepository;

        private CompanyRepository companyRepository;

        private InfrastructureDbContext _dbContext;

        public RepositoryManager(InfrastructureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICompanyRepository Company
        {
            get
            {
                if(companyRepository == null)
                {
                    companyRepository = new CompanyRepository(_dbContext);
                }

                return companyRepository;
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if(employeeRepository == null)
                {
                    employeeRepository = new EmployeeRepository(_dbContext);
                }

                return employeeRepository;
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
