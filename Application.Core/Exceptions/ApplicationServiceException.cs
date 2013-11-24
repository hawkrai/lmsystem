using System;

namespace Application.Core.Exceptions
{
    public class ApplicationServiceException : ApplicationExceptionBase
    {
        public ApplicationServiceException()
        {
        }

        public ApplicationServiceException(string message)
            : base(message)
        {
        }

        public ApplicationServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
