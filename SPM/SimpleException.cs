public class SimpleException : Exception
{
    public SimpleException() : base() { }

    public SimpleException(string message) : base(message) { }

    public SimpleException(string message, Exception innerException) : base(message, innerException) { }

    public override string StackTrace => string.Empty;
}
