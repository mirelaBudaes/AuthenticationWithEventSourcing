using System;

namespace Authentication.EventStore.Data
{
    public class LoggedEvent
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public Guid AggregateId { get; set; }
        public string Data { get; set; }
        public System.DateTime TimeStamp { get; set; }
    }
}
