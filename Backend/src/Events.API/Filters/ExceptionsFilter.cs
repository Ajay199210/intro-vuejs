using Events.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Events.API.Filters
{
    public class ExceptionsFilter : IActionFilter
    {
        private readonly ILogger<ExceptionsFilter> _logger;

        public ExceptionsFilter(ILogger<ExceptionsFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is HttpException exception)
                {
                    context.Result = new ObjectResult(exception.Errors)
                    {
                        StatusCode = exception.StatusCode
                    };

                    _logger.LogError(context.Exception, $"{DateTime.Now} [-] Failed to handle request: " + "{mdg}",
                        exception.Errors); // pour garder les traces du log dans le serveur
                }
                else
                {
                    context.Result = new ObjectResult(new ProblemDetails
                    {
                        Title = "Internal server error",
                        Status = 500,
                        Detail = context.Exception.Message
                    })
                    {
                        StatusCode = 500
                    }; // pour la reponse au client HTTP

                    _logger.LogError(context.Exception, $"{DateTime.Now} [-] Internal server error " + "{mdg}",
                        context.Exception.Message);
                }
                
                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning($"{DateTime.Now} [-] Invalid model state");
            }
        }
    }
}
