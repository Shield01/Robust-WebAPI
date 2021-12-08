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
    public class ValidateCompanyExistAttribute : IAsyncActionFilter
    {
        private readonly ILogImplementations _logImplementations;

        private readonly IRepositoryManager _repositoryManager;

        public ValidateCompanyExistAttribute(IRepositoryManager repositoryManager, ILogImplementations logImplementations)
        {
            _logImplementations = logImplementations;

            _repositoryManager = repositoryManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");

            var id = (Guid)context.ActionArguments["id"];

            var company = await _repositoryManager.Company.FindCompany(id, trackChanges);

            if(company == null)
            {
                _logImplementations.ErrorMessage($"Company id passed by client does not exist in the database");

                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("company", company);

                await next();
            }
        }
    }
}
