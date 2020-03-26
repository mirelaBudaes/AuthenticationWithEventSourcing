using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Authentication.EventStore.Models;
using LiteDB;

namespace Authentication.EventStore
{
    internal class LiteDbStore : IEventStore
    {
        private string _connectionStrong = @"C:\Work\Opdrachten\Registration\Registration\\LiteUsers.db";

        public LiteDbStore()
        {

        }
        public void X()
        {

        }

        public List<AuthenticationEvent> GetAll()
        {
            using (var db = new LiteDatabase(_connectionStrong))
            {
                var col = db.GetCollection<AuthenticationEvent>("UserEvents");

                return col.FindAll().ToList();
            }
        }

        public List<AuthenticationEvent> GetAll(Guid aggregateId)
        {
            using (var db = new LiteDatabase(_connectionStrong))
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
            using (var db = new LiteDatabase(_connectionStrong))
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
