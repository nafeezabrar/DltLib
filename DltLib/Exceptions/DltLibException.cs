using System;

namespace DltLib.Exceptions
{
    public class DltLibException : Exception
    {
        public DltLibException()
        {
        }

        public DltLibException(string message) : base(message)
        {
        }

        public DltLibException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}