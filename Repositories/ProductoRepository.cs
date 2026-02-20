using Microsoft.Data.Sqlite;
using tl2_tp8_2025_michdeaver.Models;
using tl2_tp8_2025_michdeaver.Interfaces;

namespace tl2_tp8_2025_michdeaver.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string connectionString;

        public ProductoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Producto> GetProductos()
        {
            var productos = new List<Producto>();
            string sql = "SELECT idProducto, Descripcion, Precio FROM Productos";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                productos.Add(new Producto
                {
                    IdProducto = reader.GetInt32(0),
                    Descripcion = reader.GetString(1),
                    Precio = reader.GetInt32(2)
                });
            }

            return productos;
        }

        public Producto GetProducto(int id)
        {
            string sql = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @id";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Producto
                {
                    IdProducto = reader.GetInt32(0),
                    Descripcion = reader.GetString(1),
                    Precio = reader.GetInt32(2)
                };
            }

            throw new Exception("Producto inexistente");
        }

        public void CreateProducto(Producto producto)
        {
            string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@descripcion, @precio)";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@precio", producto.Precio);

            command.ExecuteNonQuery();
        }

        public void UpdateProducto(int id, Producto producto)
        {
            string sql = @"UPDATE Productos 
                           SET Descripcion = @descripcion,
                               Precio = @precio
                           WHERE idProducto = @id";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@precio", producto.Precio);
            command.Parameters.AddWithValue("@id", id);

            int filas = command.ExecuteNonQuery();

            if (filas == 0)
                throw new Exception("Producto inexistente");
        }

        public void DeleteProducto(int id)
        {
            string sql = "DELETE FROM Productos WHERE idProducto = @id";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            int filas = command.ExecuteNonQuery();

            if (filas == 0)
                throw new Exception("Producto inexistente");
        }
    }
}