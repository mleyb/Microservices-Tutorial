using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grean.AtomEventStore;
using Store_Core.Adapters.DataAccess;

namespace Store_Core.Adapters.Atom
{
    class ReferenceDataFeedReader<T> : IEnumerable<T>
    {
        private readonly ILastReadFeedItemDAO _lastReadFeedItemDao;
        private readonly AtomFeed _atomfeed;
        private readonly LastReadFeedItem _lastReadFeedItem;

        public ReferenceDataFeedReader(ILastReadFeedItemDAO lastReadFeedItemDao, AtomFeed atomfeed)
        {
            _lastReadFeedItemDao = lastReadFeedItemDao;
            _atomfeed = atomfeed;

            _lastReadFeedItem = _lastReadFeedItemDao.FindByFeedId(atomfeed.Id);
            if (_lastReadFeedItem == null)
            {
                using (var scope = _lastReadFeedItemDao.BeginTransaction())
                {
                    _lastReadFeedItem = _lastReadFeedItemDao.Add(new LastReadFeedItem(atomfeed.Id, Guid.Empty));
                    scope.Commit();
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {

            var entries = _atomfeed.Entries.ToArray();
            if (!entries.Any())
                yield break;

            IEnumerable<AtomEntry> outStandingEntries;

            if (_lastReadFeedItem.LastEntryReadId != Guid.Empty)
                outStandingEntries = entries
                    .Reverse()
                    .SkipWhile(entry => ((Guid) entry.Id) != _lastReadFeedItem.LastEntryReadId)
                    .Skip(1);
            else
                outStandingEntries = entries.Reverse();

            foreach (var entry in outStandingEntries)
            {
                using (var scope = _lastReadFeedItemDao.BeginTransaction())
                {
                    _lastReadFeedItem.LastEntryReadId = entry.Id; 
                    _lastReadFeedItemDao.Update(_lastReadFeedItem);
                    scope.Commit();
                }

                yield return (T) entry.Content.Item;
            }

        }


        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
