using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using tl2_tp8_2025_michdeaver.Models;
using tl2_tp8_2025_michdeaver.Interfaces;

namespace tl2_tp8_2025_michdeaver.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
         private readonly string connectionString;

         public ProductoRepository(string _connectionString)
        {
            connectionString = _connectionString;
        }

        public List<Producto> GetProductos()
        {
            string queryString = "SELECT idProducto, Descripcion, Precio FROM Productos";
            List<Producto> productos = new List<Producto>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Producto producto = new Producto
                        {
                            IdProducto = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Precio = reader.GetInt32(2)
                        };

                        productos.Add(producto);
                    }
                }
                connection.Close(); 
            }

            return productos;
        }

        public Producto GetProducto(int id)
        {
            string queryString = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @id";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                var producto = new Producto();
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();
                command.Parameters.Add(new SqliteParameter("@id", id));
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto = new Producto
                        {
                            IdProducto = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Precio = reader.GetInt32(2)
                        };

                        return producto;
                    }
                }
            }

            return null;
        }

        public void CreateProducto(Producto newProducto)
        {
            string queryString = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@Descripcion", newProducto.Descripcion));
                command.Parameters.Add(new SqliteParameter("@Precio", newProducto.Precio));

                command.ExecuteNonQuery();
            }
        }

        public void UpdateProducto(int id, Producto newProducto)
        {
            string queryString = @"UPDATE Productos 
                                 SET Descripcion = @Descripcion,
                                 Precio = @Precio
                                 WHERE idProducto = @idProducto";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@Descripcion", newProducto.Descripcion));
                command.Parameters.Add(new SqliteParameter("@Precio", newProducto.Precio));
                command.Parameters.Add(new SqliteParameter("@idProducto", id));

                command.ExecuteNonQuery();
            }
        }

        public void DeleteProducto(int id)
        {
            string queryString = "DELETE FROM Productos WHERE idProducto = @idProducto";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@idProducto", id));
                command.ExecuteNonQuery();
            }
        }
    }
}