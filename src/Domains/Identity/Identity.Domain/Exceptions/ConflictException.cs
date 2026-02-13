using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
        public ConflictException(string message, Exception innerException) : base(message) { }
    }
}
