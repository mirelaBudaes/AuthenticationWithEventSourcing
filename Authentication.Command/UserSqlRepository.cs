using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Authentication.SqlStore.Configuration;
using Authentication.SqlStore.Models;
using Dapper;

namespace Authentication.Command
{
    public class UserSqlRepository
    {
        private readonly DatabaseConnectionStrings _connectionStrings;

        public UserSqlRepository(DatabaseConnectionStrings databaseConnectionStrings)
        {
            _connectionStrings = databaseConnectionStrings;
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
                               ([UserId],
                                [Email],
                                [EmailIsVerified],
                                [CreatedDate],
                                [LastUpdatedDate]
)
                VALUES(
                    @UserId,
                    @Email,
                    @EmailIsVerified,
                    @CreatedDate,
                    @LastUpdatedDate
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
                    EmailIsVerified = @EmailIsVerified,
                    LastUpdatedDate = @LastUpdatedDate
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
