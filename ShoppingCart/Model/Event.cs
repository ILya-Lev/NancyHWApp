using System;

namespace ShoppingCart.Model
{
    public struct Event
    {
        public int SequenceNumber { get; }
        public DateTime OccurredAt { get; }
        public string Name { get; }
        public object Content { get; }

        public Event(int sequenceNumber, DateTime occurredAt, string name, object content)
        {
            SequenceNumber = sequenceNumber;
            OccurredAt = occurredAt;
            Name = name;
            Content = content;
        }
    }
}
