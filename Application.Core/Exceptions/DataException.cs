using System;

namespace Application.Core.Exceptions
{
    public class DataException : ApplicationExceptionBase
    {
        public DataException()
        {
        }

        public DataException(string message)
            : base(message)
        {
        }

        public DataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
