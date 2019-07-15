using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mime;
using Etutor.BL.Resources;
using Etutor.Core.Exceptions;

namespace Etutor.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IStringLocalizer<ShareResource> _localizer;
        private readonly ILogger _logger;
        public CustomExceptionFilterAttribute(IStringLocalizer<ShareResource> localizer, ILogger logger)
        {
            _localizer = localizer;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;

            string message = context.Exception.Message;
            if (context.Exception is NotFoundException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                message = ((NotFoundException)context.Exception).isKeyLocalizer ? _localizer[message] : string.Format(_localizer["{0} was not found."], context.Exception.Message);
                _logger.LogWarning(context.Exception, "A not found error has occurred");
            }
            else if (context.Exception is DeleteFailureException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = ((DeleteFailureException)context.Exception).isKeyLocalizer ? _localizer[message] : string.Format(_localizer["Deletion of entity {0} failed."], context.Exception.Message);
                _logger.LogWarning(context.Exception, "A delete failure error has occurred");
            }
            else if (context.Exception is ValidationException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = ((ValidationException)context.Exception).isKeyLocalizer ? _localizer[message] : message;
                _logger.LogError(context.Exception, "A validation error has occurred");
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogTrace(context.Exception, "An unexpected error has occurred");
            }

            context.Result = new JsonResult(new
            {
                ex = context.Exception,
                message
            });
        }
    }
}

