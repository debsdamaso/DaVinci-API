namespace DaVinci.Configuration
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }

        // Construtor para garantir que as propriedades sejam inicializadas
        public MongoDbConfig(string connectionString, string database)
        {
            ConnectionString = connectionString;
            Database = database;
        }
    }
}
