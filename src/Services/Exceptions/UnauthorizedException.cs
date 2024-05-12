namespace LibraryApp.Services.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() { }
        public UnauthorizedException(string message) : base(message) { }
    }
}
