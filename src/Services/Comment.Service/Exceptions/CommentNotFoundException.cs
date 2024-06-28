namespace Comment.Service.Exceptions
{
    public class CommentNotFoundException : Exception
    {
        public CommentNotFoundException(string id) : base($"ID: {id} comment was not found")
        {
        }
    }
}
