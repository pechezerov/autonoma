using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;

namespace Autonoma.API.Shared.Infrastructure
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            IActionResult? result = null;
            switch (context.Exception)
            {
                case ValidationException ve:
                    result = CreateBadRequest(ve.Message);
                    break;
                case OperationCanceledException:
                    if (context.HttpContext.RequestAborted.IsCancellationRequested)
                    {
                        result = new EmptyResult();
                        _logger.LogDebug("Request has been canceled by client");
                    }
                    break;
            }

            if (result != null)
            {
                context.Result = result;
                context.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// Создает такую же структуру ошибки, как и в случае с ошибкой десериализации входных параметров действия контроллера
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        private static BadRequestObjectResult CreateBadRequest(string message)
        {
            var ms = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            ms.AddModelError("", message);
            return new BadRequestObjectResult(ms);
        }
    }
}
