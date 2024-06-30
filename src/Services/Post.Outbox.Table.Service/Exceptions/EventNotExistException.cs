namespace Post.Outbox.Table.Service
{
    public class EventNotExistException : Exception
    {
        public EventNotExistException(string type) : base($"type of {type} is not found")
        {
        }
    }
}
