using System;

namespace Etutor.Core.Exceptions
{
    public class DeleteFailureException : Exception
    {
        public bool isKeyLocalizer;
        public DeleteFailureException(string message, bool isKeyLocalizer = false) : base(message)
        {
            this.isKeyLocalizer = isKeyLocalizer;
        }
    }
}
