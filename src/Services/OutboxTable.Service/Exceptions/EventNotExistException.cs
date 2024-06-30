using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Tables.Sevice.Exceptions
{
    public class EventNotExistException : Exception
    {
        public EventNotExistException(string type) : base($"type of {type} is not found")
        {
        }
    }
}
