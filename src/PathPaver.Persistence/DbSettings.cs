namespace PathPaver.Persistence;

/**
 * This class is used to handle appsettings.json atlas cluster settings
 *
 * Because PathPaver.Web have an assembly reference to PathPaver.Persistence, I can specify
 * that MongoCluster section in appsettings.json will be stored in config file DbSettings.cs
 */
public class DbSettings
{
    public static string ConnectionURI { get; set; }
    public static string DatabaseName { get; set; }
}