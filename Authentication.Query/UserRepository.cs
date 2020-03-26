using System;
using System.Collections.Generic;
using Authentication.SqlStore.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Authentication.SqlStore.Models;

namespace Authentication.Query
{
    public class UserRepository
    {
        private readonly DatabaseConnectionStrings _connectionStrings;

        public UserRepository(DatabaseConnectionStrings databaseConnectionStrings)
        {
            _connectionStrings = databaseConnectionStrings;
        }

        public bool UserExists(string email)
        {
            using (IDbConnection db = GetConnection())
            {
                var parameters = new { Email = email };

                var users = db.Query<User>(
                    @"Select * From Users 
                        Where email = @email",
                    parameters
                    ).ToList();

                return users.Any();
            }
        }

        public List<User> GetLastUpdatedUsers(int topX)
        {
            using (IDbConnection db = GetConnection())
            {
                var parameters = new { MaxLimit = topX };

                var users = db.Query<User>(
                    @"Select TOP {=MaxLimit} * From Users",
                    parameters
                ).ToList();

                return users;
            }
        }


        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionStrings.UsersSqlDbConnection());
        }
    }
}
