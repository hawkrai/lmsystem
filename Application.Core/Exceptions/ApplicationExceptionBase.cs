using System;

namespace Application.Core.Exceptions
{
    public class ApplicationExceptionBase : ApplicationException
    {
        protected ApplicationExceptionBase(string message)
            : base(message)
        {
        }

        protected ApplicationExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ApplicationExceptionBase()
        {
        }
    }
}
