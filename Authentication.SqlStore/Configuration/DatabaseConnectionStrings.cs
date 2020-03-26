namespace Authentication.SqlStore.Configuration
{
    public class DatabaseConnectionStrings
    {
        public string UsersSqlDbConnection()
        {
            var setting = ConfigHelper.GetConfig();

            var connectionstring = setting["UsersSqlDbConnection"];

            return connectionstring;
        }
    }
}
