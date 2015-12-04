using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Core.Exceptions;

namespace WebServicesRole
{
    public class ControllerActionInvoker : ApiControllerActionInvoker
    {
        public override Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> result = base.InvokeActionAsync(actionContext, cancellationToken);

            if (result.Exception != null)
            {
                Exception baseException = result.Exception.GetBaseException();

                if (baseException is NotFoundException)
                {
                    NotFoundException exceptionBase = baseException as NotFoundException;
                    HttpResponseMessage responseMessage = actionContext.Request.CreateResponse(HttpStatusCode.NotFound, exceptionBase.Message);
                    responseMessage.ReasonPhrase = "Not Found";
                    return Task.Run(() => responseMessage, cancellationToken);
                }
                else if (baseException is InvalidValueException)
                {
                    InvalidValueException exceptionBase = baseException as InvalidValueException;
                    HttpResponseMessage responseMessage = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, exceptionBase.Message);
                    responseMessage.ReasonPhrase = "Bad Request";
                    return Task.Run(() => responseMessage, cancellationToken);
                }
                else if (baseException is UnauthorizedAccessException)
                {
                    HttpResponseMessage responseMessage = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    responseMessage.ReasonPhrase = "Unauthorized";
                    return Task.Run(() => responseMessage, cancellationToken);
                }
                else
                {
                    HttpResponseMessage responseMessage = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError, baseException.Message);
                    responseMessage.ReasonPhrase = "Internal Server Error";
                    return Task.Run(() => responseMessage, cancellationToken);
                }
            }
            return result;
        }
    }
}