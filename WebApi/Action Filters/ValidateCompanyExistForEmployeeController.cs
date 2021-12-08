using Infrastructure.Repository_Manager;
using LogService.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Action_Filters
{
    public class ValidateCompanyExistForEmployeeController : IAsyncActionFilter
    {
        private readonly ILogImplementations _logImplementations;

        private readonly IRepositoryManager _repositoryManager;

        public ValidateCompanyExistForEmployeeController(IRepositoryManager repositoryManager, ILogImplementations logImplementations)
        {
            _logImplementations = logImplementations;

            _repositoryManager = repositoryManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;

            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;

            var companyId = (Guid)context.ActionArguments["companyId"];

            var company = await _repositoryManager.Company.FindCompany(companyId, false);

            if (company == null)
            {
                _logImplementations.ErrorMessage($"Company id passed by client does not exist in the database");

                context.Result = new NotFoundResult();

                return;
            }

            var id = (Guid)context.ActionArguments["id"];

            var employee = await _repositoryManager.Employee.GetAnEmployeeFromACompany(companyId, id, trackChanges);

            if(employee == null)
            {
                _logImplementations.ErrorMessage($"Employee with Id : {id}, does not exist in the database");

                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("employee", employee);

                await next();
            }
        }
    }
}
