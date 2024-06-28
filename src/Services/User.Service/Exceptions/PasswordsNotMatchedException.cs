namespace User.Service.Exceptions
{
    public class PasswordsNotMatchedException : Exception
    {
        public PasswordsNotMatchedException() : base("password and confirm password must have matched")
        {
        }
    }
}
