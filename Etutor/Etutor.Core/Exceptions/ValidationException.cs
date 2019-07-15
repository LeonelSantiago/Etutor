using System;

namespace Etutor.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public bool isKeyLocalizer;
        public ValidationException(string message, bool isKeyLocalizer = false) : base(message)
        {
            this.isKeyLocalizer = isKeyLocalizer;
        }
    }
}
