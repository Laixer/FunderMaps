using System;

namespace FunderMaps.Core.Exceptions
{
    public class EntityNotFoundException : FunderMapsCoreException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public EntityNotFoundException()
        {
        }
    }
}
