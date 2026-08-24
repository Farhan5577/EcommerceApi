namespace EcommerceApi.Options.Exceptions
{
    public sealed class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
