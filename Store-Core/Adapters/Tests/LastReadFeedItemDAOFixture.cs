using System;
using Machine.Specifications;
using Store_Core.Adapters.DataAccess;
using Store_Core.Adapters.Atom;

namespace Store_Core.Adapters.Tests
{
    [Subject(typeof(LastReadFeedItemDAO))]
    public class When_updating_an_order
    {
        private static LastReadFeedItemDAO s_dao;
        private static LastReadFeedItem s_newFeed;
        private static LastReadFeedItem  s_updatedFeed;
        private static LastReadFeedItem s_foundFeed;

        private Establish _context = () =>
        {
            s_dao = new LastReadFeedItemDAO();
            s_dao.Clear();
            s_newFeed = new LastReadFeedItem(feedId: new Guid("{6D63F2D1-1C71-4150-8857-190CB680F822}"), lastEntryReadId:new Guid("{D3669787-5B7F-4CA9-A844-40204C6E3B1E}"));
            s_newFeed = s_dao.Add(s_newFeed);
            s_updatedFeed = new LastReadFeedItem(feedId:s_newFeed.FeedId, lastEntryReadId: new Guid("{FF7AD12A-24AA-4E79-8959-980D21A7D8E0}")) {Id = s_newFeed.Id};
        };

        private Because _of = () => s_dao.Update(s_updatedFeed);

        private It _should_add_the_feed_into_the_list = () => GetLastReadFeed().ShouldNotBeNull();
        private It _should_set_the_last_read_item = () => GetLastReadFeed().LastEntryReadId.ShouldEqual(s_updatedFeed.LastEntryReadId);

        private static LastReadFeedItem GetLastReadFeed()
        {
            return s_foundFeed ?? (s_foundFeed = s_dao.FindByFeedId(s_newFeed.FeedId));
        }
    }
}
