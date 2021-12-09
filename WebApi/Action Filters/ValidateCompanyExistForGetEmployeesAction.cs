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
    public class ValidateCompanyExistForGetEmployeesAction : IActionFilter
    {
        private readonly ILogImplementations _logimplementations;
        private readonly IRepositoryManager _repositoryManager;

        public ValidateCompanyExistForGetEmployeesAction(ILogImplementations logImplementations, IRepositoryManager repositoryManager)
        {
            _logimplementations = logImplementations;

            _repositoryManager = repositoryManager;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var method = context.HttpContext.Request.Method;

            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;

            var companyId = (Guid)context.ActionArguments["companyId"];

            var company =  _repositoryManager.Company.FindCompany(companyId, false);

            if(company == null)
            {
                _logimplementations.ErrorMessage($"Company Id : {companyId} passed by client does not exist in the database");

                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("company", company);
            }
        }
    }
}
