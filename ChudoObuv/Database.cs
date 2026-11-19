using Npgsql;

namespace ChudoObuv
{
    public static class Database
    {
        private const string ConnectionString =
            "Host=localhost;Port=5432;Database=shoes_shop_db;Username=postgres;Password=1111";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }
    }
}