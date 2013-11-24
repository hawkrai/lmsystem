using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Exceptions
{
	public class ApplicationUIException : ApplicationExceptionBase
	{
		public ApplicationUIException()
        {
        }

        public ApplicationUIException(string message)
            : base(message)
        {
        }

		public ApplicationUIException(string message, Exception inner)
            : base(message, inner)
        {
        }
	}
}
