using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using LogService.Abstractions;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using BusinessLogic.Exception_Handling.Exception_Models;

namespace BusinessLogic.Exception_Handling
{
    public static class ExceptionExtensions
    {
        public static void ConfigureExceptions(this IApplicationBuilder applicationBuilder, ILogImplementations logImplementations)
        {
            applicationBuilder.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        logImplementations.ErrorMessage($"Something went wrong in the {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,

                            Message = "Internal Server Error"
                        }.ToString());
                    }
                });
            });
        }
    }
}
