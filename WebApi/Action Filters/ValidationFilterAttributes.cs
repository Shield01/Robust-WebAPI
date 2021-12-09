using Infrastructure.Data_Transfer_Objects;
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
    public class ValidationFilterAttributes : IActionFilter
    {

        private readonly ILogImplementations _logImplementations;

        public ValidationFilterAttributes(ILogImplementations logImplementations)
        {
            _logImplementations = logImplementations;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];

            var controller = context.RouteData.Values["controller"];

            var parameter = context.ActionArguments.SingleOrDefault(x => x.Value is CompanyManipulationClass || x.Value is EmployeeManipulationClass || x.Value is IEnumerable<CompanyManipulationClass> || x.Value is IEnumerable<EmployeeManipulationClass>).Value;

            if (parameter == null)
            {
                _logImplementations.ErrorMessage($"Object sent from the client is null. Controller : {controller}, Action : {action}");

                context.Result = new BadRequestObjectResult($"Object is null, Controller : {controller}, Action : {action}");
            }

            if (!context.ModelState.IsValid)
            {
                _logImplementations.ErrorMessage($"Model state of the object passed by the client is not valid. Controller : {controller}, Action : {action}");

                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    var action = context.RouteData.Values["action"];

        //    var controller = context.RouteData.Values["controller"];

        //    var parameter = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;

        //    if (parameter == null)
        //    {
        //        _logImplementations.ErrorMessage($"Object sent from the client is null. Controller : {controller}, Action : {action}");

        //        context.Result = new BadRequestObjectResult($"Object is null, Controller : {controller}, Action : {action}");
        //    }

        //    if (!context.ModelState.IsValid)
        //    {
        //        _logImplementations.ErrorMessage($"Model state of the object passed by the client is not valid. Controller : {controller}, Action : {action}");

        //        context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        //    }
        //    else
        //    {
        //        await next();
        //    }
        //}
    }
}
