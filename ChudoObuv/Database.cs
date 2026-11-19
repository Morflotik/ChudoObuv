
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
                    p.id,
                    p.product_name,
                    c.category_name,
                    m.manufacturer_name,
                    p.image_url,
                    p.description,
                    p.composition,
                    p.price,
                    COALESCE(SUM(s.available_quantity), 0) AS stock_quantity,
                    CASE
                        WHEN NOT EXISTS (
                            SELECT 1
                            FROM order_items oi
                            JOIN orders o ON o.id = oi.order_id
                            WHERE oi.product_id = p.id
                              AND o.order_date >= date_trunc('month', CURRENT_DATE - INTERVAL '1 month')
                              AND o.order_date < date_trunc('month', CURRENT_DATE)
                        )
                        THEN 0.75
                        ELSE 1.0
                    END AS discount_multiplier
                FROM products p
                LEFT JOIN categories c ON c.id = p.category_id
                LEFT JOIN manufacturers m ON m.id = p.manufacturer_id
                LEFT JOIN stock s ON s.product_id = p.id
                GROUP BY
                    p.id,
                    p.product_name,
                    c.category_name,
                    m.manufacturer_name,
                    p.image_url,
                    p.description,
                    p.composition,
                    p.price
                ORDER BY p.product_name;
            ";

            using var adapter = new NpgsqlDataAdapter(sql, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}
