using System;
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

        internal User FindById(Guid userId)
        {
            using (IDbConnection db = GetConnection())
            {
                var parameters = new { UserId = userId };

                var users = db.Query<User>(
                    @"Select * From Users 
                        Where UserId = @userId",
                    parameters
                ).ToList();

                return users.FirstOrDefault();
            }
        }

        public void Save(User user)
        {
            var existing = FindById(user.UserId);
            if (existing == null)
            {
                InsertUser(user);
            }
            else
            {
                UpdateUser(user);
            }
        }

        private void InsertUser(User user)
        {
            var insertCommand = $@"

                INSERT INTO [dbo].[Users]
                               ([UserId]
                               ,[Email]
                                , [EmailIsVerified])
                VALUES(
                    @UserId,
                    @Email,
                    @EmailIsVerified
                    )";

            using (IDbConnection db = GetConnection())
            {
                db.Execute(insertCommand, user);
            }
        }

        private void UpdateUser(User changedUser)
        {
            var updateCommand = $@"
                UPDATE [dbo].[Users]
                SET Email = @Email,
                    EmailIsVerified = @EmailIsVerified
                WHERE UserId = @UserId";

            using (IDbConnection db = GetConnection())
            {
                db.Execute(updateCommand, changedUser);
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionStrings.UsersSqlDbConnection());
        }
    }
}
