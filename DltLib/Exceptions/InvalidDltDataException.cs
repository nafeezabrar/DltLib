using System;

namespace DltLib.Exceptions
{
    public class InvalidDltDataException : DltLibException
    {
        public InvalidDltDataException()
        {
        }

        public InvalidDltDataException(string message) : base(message)
        {
        }

        public InvalidDltDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}