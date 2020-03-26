using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore.Configuration;
using Authentication.EventStore.Models;
using LiteDB;

namespace Authentication.EventStore
{
    internal class LiteDbStore : IEventStore
    {
        private readonly DatabaseConnectionStrings _databaseConnectionStrings;

        public LiteDbStore(DatabaseConnectionStrings databaseConnectionStrings)
        {
            _databaseConnectionStrings = databaseConnectionStrings;
        }
      
        public List<AuthenticationEvent> GetAll()
        {
            using (var db = new LiteDatabase(_databaseConnectionStrings.LiteDbConnection()))
            {
                var col = db.GetCollection<AuthenticationEvent>("UserEvents");

                return col.FindAll().ToList();
            }
        }

        public List<AuthenticationEvent> GetAll(int topX)
        {
            using (var db = new LiteDatabase(_databaseConnectionStrings.LiteDbConnection()))
            {
                var col = db.GetCollection<AuthenticationEvent>("UserEvents")
                    .Query()
                    .OrderBy(x => x.TimeStamp)
                    .Limit(topX);

                return col.ToList();
            }
        }

        public List<AuthenticationEvent> GetAll(Guid aggregateId)
        {
            using (var db = new LiteDatabase(_databaseConnectionStrings.LiteDbConnection()))
            {
                var col = db.GetCollection<AuthenticationEvent>("UserEvents");

                var results = col.Query()
                    .Where(x => x.UserId == aggregateId)
                    .ToList();

                return results;
            }
        }

        public void Save(AuthenticationEvent newEvent)
        {
            using (var db = new LiteDatabase(_databaseConnectionStrings.LiteDbConnection()))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<AuthenticationEvent>("UserEvents");

                // Insert new customer document (Id will be auto-incremented)
                col.Insert(newEvent);

                col.EnsureIndex(x => x.UserId);

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
