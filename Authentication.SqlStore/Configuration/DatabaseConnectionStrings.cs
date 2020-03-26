namespace Authentication.SqlStore.Configuration
{
    public class DatabaseConnectionStrings
    {
        public string UsersSqlDbConnection()
        {
            var setting = ConfigHelper.GetConfig();

            return setting["UsersSqlDbConnection"];
        }
    }
}
