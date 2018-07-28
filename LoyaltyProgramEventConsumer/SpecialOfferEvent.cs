namespace LoyaltyProgramEventConsumer
{
    internal class SpecialOfferEvent
    {
        public int SequenceNumber { get; set; }
        public string Name { get; set; }
        public object Content { get; set; }
    }
}