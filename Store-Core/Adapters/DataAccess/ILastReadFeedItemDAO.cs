using System;
using System.Collections.Generic;
using Store_Core.Adapters.Atom;

namespace Store_Core.Adapters.DataAccess
{
    public interface ILastReadFeedItemDAO
    {
        dynamic BeginTransaction();
        LastReadFeedItem Add(LastReadFeedItem  newLastReadFeedItem);
        void Clear();
        void Delete(int productId);
        IEnumerable<LastReadFeedItem > FindAll();
        LastReadFeedItem FindByFeedId(Guid id);
        void Update(LastReadFeedItem lastReadFeedItem);
    }
}
