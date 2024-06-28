namespace User.Service.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string id) : base($"ID = {id} user is not found")
        {
        }
    }
}
