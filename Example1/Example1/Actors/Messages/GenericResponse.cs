using System;

namespace Example1.Actors.Messages
{
    public class GenericResponseMessage
    {
        public bool Successful { get; }
        public string Message { get; }

        public GenericResponseMessage(bool successful, string message)
        {
            Successful = successful;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

    }
}