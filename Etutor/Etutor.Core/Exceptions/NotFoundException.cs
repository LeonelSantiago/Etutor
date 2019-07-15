using System;

namespace Etutor.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public bool isKeyLocalizer;
        public NotFoundException(string message, bool isKeyLocalizer = false) : base(message)
        {
            this.isKeyLocalizer = isKeyLocalizer;
        }
    }
}
