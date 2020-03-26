using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore.Configuration;
using LiteDB;

namespace Authentication.EventStore.Data
{
    internal class LiteDbStore : IEventStore
    {
        private readonly DatabaseConnectionStrings _databaseConnectionStrings;

        public LiteDbStore(DatabaseConnectionStrings databaseConnectionStrings)
        {
            _databaseConnectionStrings = databaseConnectionStrings;
        }
      
        public List<LoggedEvent> GetAll()
        {
            using (var db = new LiteDatabase(_databaseConnectionStrings.LiteDbConnection()))
            {
                var col = db.GetCollection<LoggedEvent>("UserEvents");

                return col.FindAll().ToList();
            }
        }

        public List<LoggedEvent> GetAll(int topX)
        {
            using (var db = new LiteDatabase(_databaseConnectionStrings.LiteDbConnection()))
            {
                var col = db.GetCollection<LoggedEvent>("UserEvents")
                    .Query()
                    .OrderBy(x => x.TimeStamp)
                    .Limit(topX);

                return col.ToList();
            }
        }

        public List<LoggedEvent> GetAll(Guid aggregateId)
        {
            using (var db = new LiteDatabase(_databaseConnectionStrings.LiteDbConnection()))
            {
                var col = db.GetCollection<LoggedEvent>("UserEvents");

                var results = col.Query()
                    .Where(x => x.AggregateId == aggregateId)
                    .ToList();

                return results;
            }
        }

        public void Save(LoggedEvent newEvent)
        {
            using (var db = new LiteDatabase(_databaseConnectionStrings.LiteDbConnection()))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<LoggedEvent>("UserEvents");

                // Insert new customer document (Id will be auto-incremented)
                col.Insert(newEvent);

                col.EnsureIndex(x => x.AggregateId);

                //// Use LINQ to query documents (filter, sort, transform)
                //var results = col.Query()
                //    .Where(x => x.Name.StartsWith("J"))
                //    .OrderBy(x => x.Name)
                //    .Select(x => new { x.Name, NameUpper = x.Name.ToUpper() })
                //    .Limit(10)
                //    .ToList();
            }
        }
    }
}
