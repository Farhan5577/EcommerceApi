namespace EcommerceApi.Options.Exceptions
{
    public sealed class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
