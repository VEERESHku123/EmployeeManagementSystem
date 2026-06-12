namespace Frontend.Exceptions
{
    public class SessionExpiredException : Exception
    {
        public SessionExpiredException() : base("Session expired")
        {
        }
    }
}
