using System;
using Grean.AtomEventStore;

namespace Store_Core.Adapters.Atom
{
    public class LastReadFeedItem
    {
        public Guid FeedId { get; set; }
        public int Id { get; set; }
        public Guid LastEntryReadId { get; set; }

        public LastReadFeedItem() { /*Required for Simple Data*/ }

        public LastReadFeedItem(Guid feedId, Guid lastEntryReadId)
        {
            FeedId = feedId;
            LastEntryReadId = lastEntryReadId;
        }
    }
}
