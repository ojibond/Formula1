using AutoMapper;
using FormulaOne.Api.ApiResources;
using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;

namespace FormulaOne.Api.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        protected string NullParamDefaultErrorMessage = "param is required and cannot be null nor empty!";

        private readonly IEnumerable<string> _notFoundExceptionPhrases = new List<string>() { "not found", "does not exist", "could not find" };
        private readonly IEnumerable<string> _notAuthorizedExceptionPhrases = new List<string>() { "invalid username", "invalid password" };


        public BaseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected virtual IActionResult ReturnRestfulResultForThrownException(Exception ex)
        {
            return ReturnRestfulResultForThrownException(ex, additionalMessage: null, additionalData: null);
        }

        protected virtual IActionResult ReturnRestfulResultForThrownException(Exception ex, object additionalData)
        {
            return ReturnRestfulResultForThrownException(ex, additionalMessage: null, additionalData: null);
        }


        private IActionResult ReturnRestfulResultForThrownException(Exception ex, string additionalMessage, object additionalData)
        {
            if (ex as ArgumentNullException != null)
            {
                if (ex.Message.Contains(NullParamDefaultErrorMessage))
                {
                    return RestResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }

            if (ex as AuthenticationException != null
                || ex as UnauthorizedAccessException != null)
            {
                return RestResponse(HttpStatusCode.Unauthorized, ex.Message);
            }

            if (ex as FileNotFoundException != null)
            {
                return ReturnNotFoundResultStatus(ex.Message);
            }

            // additional attempts at capturing applicable http status error codes - looking for partial text matches in the error message
            if (_notFoundExceptionPhrases.Any(x => ex.Message.Contains(x, StringComparison.InvariantCultureIgnoreCase)))
            {
                return ReturnNotFoundResultStatus(ex.Message);
            }
            if (_notAuthorizedExceptionPhrases.Any(x => ex.Message.Contains(x, StringComparison.InvariantCultureIgnoreCase)))
            {
                return RestResponse(HttpStatusCode.Unauthorized, ex.Message);
            }

            // if no other matches were found above return 500           
            if (ex as AggregateException != null)
            {
                Exception? useEx = (ex as AggregateException).Flatten().InnerException;
                return RestResponse(HttpStatusCode.InternalServerError, useEx.InnerException.Message);
            }

            return RestResponse(HttpStatusCode.InternalServerError, ex.Message);
        }

        protected IActionResult NotYetImplementedEndPoint => NotYetImplementedResultStatus("This end point has not yet been built-out.");

        protected IActionResult NotYetImplementedResultStatus(string message) => RestResponse(HttpStatusCode.NotImplemented, message);

        protected IActionResult ReturnNotFoundResultStatus(string message) => RestResponse(HttpStatusCode.NotFound, message);

        protected IActionResult ReturnBadRequestResultStatus() => RestResponse(HttpStatusCode.BadRequest, message: null);

        protected IActionResult RestResponse(HttpStatusCode statusCode, string? message = null)
        {
            if ((int)statusCode < 400)
            {
                return CreateSuccessfulRestResponse(statusCode, message);
            }
            return CreateErrorRestResponse(statusCode, message ?? string.Empty);
        }

        #region Private methods
        private IActionResult CreateSuccessfulRestResponse(HttpStatusCode statusCode, string message = null)
        {
            var response = new HttpResponseMessage(statusCode);

            if (!string.IsNullOrEmpty(message))
            {
                var useMessage = RemoveSpecialCharacters(message);
                response = response.WithReasonPhrase((string)useMessage);
            }
            return Ok(response);
        }


        private IActionResult CreateErrorRestResponse(HttpStatusCode statusCode, string message)
        {
            var response = new HttpResponseMessage(statusCode);

            if (!string.IsNullOrEmpty(message))
            {
                var useMessage = RemoveSpecialCharacters(message);
                response = response.WithReasonPhrase((string)useMessage);
            }
            return Ok(response);
        }

        private object RemoveSpecialCharacters(string message)
        {
            //clean message of any carrage return/line feeds
            return message.Replace(System.Environment.NewLine, " | ");
        }

        #endregion
    }
}
