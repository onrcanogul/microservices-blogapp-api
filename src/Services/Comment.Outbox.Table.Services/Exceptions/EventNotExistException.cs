namespace Comment.Outbox.Table.Services
{
    public class EventNotExistException : Exception
    {
        public EventNotExistException(string type) : base($"type of {type} is not found")
        {
        }
    }
}
