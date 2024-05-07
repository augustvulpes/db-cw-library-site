namespace LibraryApp.Services.Exceptions
{
    public class UnprocessableException : Exception
    {
        public UnprocessableException() { }
        public UnprocessableException(string message) : base(message) { }
    }
}
