using Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository_Manager
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }

        IEmployeeRepository Employee { get; }

        void Save();
    }
}
