namespace Post.Service.Exceptions
{
    public class PostNotFoundException : Exception
    {
        public PostNotFoundException(object key) : base($"ID = ${key} post is not found")
        {
        }
    }
}
