using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grean.AtomEventStore;

namespace Store_Core.Adapters.Atom
{
    class ReferenceDataFeedReader<T> : IEnumerable<T>
    {
        private string _atomfeed;

        public ReferenceDataFeedReader(string atomfeed)
        {
            _atomfeed = atomfeed;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var serializer = new DataContractContentSerializer(DataContractContentSerializer.CreateTypeResolver(typeof (ProductEntry).Assembly));
            var feed = AtomFeed.Parse(_atomfeed, serializer);

            var entries = feed.Entries.ToArray();
            if (!entries.Any())
                yield break;

            foreach (var entry in entries)
                yield return (T)entry.Content.Item;
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
