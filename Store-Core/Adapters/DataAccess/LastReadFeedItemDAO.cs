using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Simple.Data;
using Store_Core.Adapters.Atom;

namespace Store_Core.Adapters.DataAccess
{
    public class LastReadFeedItemDAO : ILastReadFeedItemDAO
    {
        private readonly dynamic _db;

        public LastReadFeedItemDAO()
        {
            if (System.Web.HttpContext.Current != null)
            {
                var databasePath = System.Web.HttpContext.Current.Server.MapPath("~\\App_Data\\Store.sdf");
                _db = Database.Opener.OpenFile(databasePath);
            }
            else
            {
                var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "App_Data\\Store.sdf");

                _db = Database.OpenFile(file);
            }
        }

        public dynamic BeginTransaction()
        {
            return _db.BeginTransaction();
        }

        public LastReadFeedItem Add(LastReadFeedItem lastReadFeedItem)
        {
            return _db.LastReadFeedItem.Insert(lastReadFeedItem);
        }

        public void Clear()
        {
            _db.LastReadFeedItem.DeleteAll();
        }

        public void Delete(int lastFeedItemId)
        {
            _db.LastReadFeedItem.DeleteById(lastFeedItemId);
        }

        public IEnumerable<LastReadFeedItem > FindAll()
        {
            return _db.LastReadFeedItem.All().ToList<LastReadFeedItem >();
        }

        public LastReadFeedItem FindByFeedId(Guid id)
        {
            return _db.LastReadFeedItem.FindByFeedId(id);
        }

        public void Update(LastReadFeedItem productReference)
        {
            _db.LastReadFeedItem.UpdateById(productReference);
        }

    }
}
