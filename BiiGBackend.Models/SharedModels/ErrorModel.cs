using BiiGBackend.SharedModels.Enums;
using System.Net;

namespace TutorApplication.SharedModels.Models
{


    public class ErrorModel
    {
        public string StatusCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;

        private const string ErrorKeyWord = "ERROR";
        private const string MessageKeyWord = "MESSAGE";
        public ErrorModel(string statusCode, string message, string stackTrace)
        {

            StatusCode = statusCode;
            Message = message;
            StackTrace = stackTrace;

            string identifierKeyWord = "";
            string errorMessage = "";
            if (message.Contains(":"))
            {
                identifierKeyWord = message.Split(":")[0];
                errorMessage = message.Split(":")[1];
            }
            else
            {
                identifierKeyWord = "UnclassifiedError";
                errorMessage = message;
            }



            ErrorIdentifiers identifier;
            Enum.TryParse(identifierKeyWord, out identifier);

            ErrorCodes errorStatus;
            Enum.TryParse(errorMessage, out errorStatus);

            switch (identifier)
            {
                case ErrorIdentifiers.CustomError:
                    StatusCode = ErrorDictionary[errorStatus][ErrorKeyWord];
                    Message = ErrorDictionary[errorStatus][MessageKeyWord];
                    break;
                case ErrorIdentifiers.ValidationErrors:
                    StatusCode = ((int)HttpStatusCode.Unauthorized).ToString();
                    Message = errorMessage;
                    break;
                case ErrorIdentifiers.UnclassifiedError:
                    StatusCode = ((int)HttpStatusCode.InternalServerError).ToString();
                    Message = errorMessage;
                    break;
                default:
                    StatusCode = ((int)HttpStatusCode.InternalServerError).ToString();
                    Message = errorMessage;
                    break;
            }
        }

        private Dictionary<ErrorCodes,
            Dictionary<string, string>
            > ErrorDictionary => new Dictionary<ErrorCodes, Dictionary<string, string>>()
            {
                {
                    ErrorCodes.IncorrectPassword,
                    new()
                    {
                        {MessageKeyWord,"Password is incorrect"},
                        {ErrorKeyWord,((int)HttpStatusCode.BadRequest).ToString() }
                    }
                },
                {
                    ErrorCodes.UserDoesNotExist,
                    new()
                    {
                        {MessageKeyWord,"User does not exist"},
                        {ErrorKeyWord,((int)HttpStatusCode.BadRequest).ToString() }
                    }
                },
                {
                    ErrorCodes.UserExist,
                    new()
                    {
                        {MessageKeyWord,"User already exists"},
                        {ErrorKeyWord,((int)HttpStatusCode.BadRequest).ToString() }
                    }
                },
                {
                    ErrorCodes.ProductDeleted,
                    new()
                    {
                        {MessageKeyWord,"Product has been deleted"},
                        {ErrorKeyWord,((int)HttpStatusCode.NotFound).ToString() }
                    }
                },

                {
                    ErrorCodes.ErrorWhileSaving,
                    new()
                    {
                        {MessageKeyWord,"Did not save sucessfully"},
                        {ErrorKeyWord,((int)HttpStatusCode.BadRequest).ToString() }
                    }
                },

                {
                    ErrorCodes.ErrorWhileAdding,
                    new()
                    {
                        {MessageKeyWord,"Did not add sucessfully"},
                        {ErrorKeyWord,((int)HttpStatusCode.BadRequest).ToString() }
                    }
                },

                {
                    ErrorCodes.CourseDoesNotExist,
                    new()
                    {
                        {MessageKeyWord,"Course Does Not Exist"},
                        {ErrorKeyWord,((int)HttpStatusCode.BadRequest).ToString() }
                    }
                },

                {
                    ErrorCodes.CartItemExist,
                    new()
                    {
                        {MessageKeyWord,"This item has already been added to the cart"},
                        {ErrorKeyWord,((int)HttpStatusCode.BadRequest).ToString() }
                    }
                },
                    {
                    ErrorCodes.NotAuthenticatedLogin,
                    new()
                    {
                        {MessageKeyWord,"Login First"},
                        {ErrorKeyWord,((int)HttpStatusCode.Forbidden).ToString() }
                    }
                },

                {
                    ErrorCodes.UserAuthDoesNotExist,
                    new()
                    {
                        {MessageKeyWord,"User Auth Does Not Exist"},
                        {ErrorKeyWord,((int)HttpStatusCode.BadRequest).ToString() }
                    }
                },
                {
                    ErrorCodes.CartItemNotRetrievable,
                    new()
                    {
                        {MessageKeyWord,"Cart is not authenticated"},
                        {ErrorKeyWord,((int)HttpStatusCode.Forbidden).ToString() }
                    }
                },





            };
    }
}
