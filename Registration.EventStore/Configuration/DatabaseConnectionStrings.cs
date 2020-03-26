namespace Authentication.EventStore.Configuration
{
    public class DatabaseConnectionStrings
    {
        public string LiteDbConnection()
        {
            var setting = ConfigHelper.GetConfig();

            return setting["LiteDbStoreConnection"];
        }
    }
}
