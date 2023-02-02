using System;
using System.Runtime.Serialization;

[Serializable]
internal class InvalidCodeException : Exception
{
    public InvalidCodeException()
    {
    }

    public InvalidCodeException(string message) : base(message)
    {
    }

    public InvalidCodeException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected InvalidCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}