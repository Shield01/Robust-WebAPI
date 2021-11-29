using AutoMapper;
using Core.Models;
using Infrastructure.Data_Transfer_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDTO>().
                ForMember(c => c.FullAddress, 
                opt => opt.MapFrom(x => string.Join(" ", x.Address, x.Country)));

            CreateMap<Employee, EmployeeDTO>();

            CreateMap<CompanyInputDTO, Company>();

            CreateMap<EmployeeInputDTO, Employee>();
        }
    }
}
