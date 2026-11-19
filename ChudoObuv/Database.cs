using Npgsql;
using System.Data;

namespace ChudoObuv
{
    public static class Database
    {
        private const string ConnectionString =
            "Host=localhost;Port=5432;Database=shoes_shop_db;Username=postgres;Password=MyShoes2026!";

        public static DataTable GetProducts()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            string sql = @"
                SELECT
                    id,
                    product_name,
                    category_id,
                    subcategory_id,
                    manufacturer_id,
                    image_url,
                    description,
                    composition,
                    price
                FROM products
                ORDER BY product_name;
            ";

            using var adapter = new NpgsqlDataAdapter(sql, connection);

            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}