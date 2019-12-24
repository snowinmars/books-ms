using System;
using System.Runtime.Serialization;

namespace BookService.CommonEntities.Exceptions
{
    // ReSharper disable once AllowPublicClass
    public class NotFoundException : BusinessException
    {
        public NotFoundException() { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
