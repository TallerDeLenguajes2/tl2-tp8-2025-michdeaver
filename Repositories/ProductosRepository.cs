using Microsoft.Data.Sqlite;

public class ProductosRepository
{
    string connectionString = "Data Source=db/Tienda.db";
    public List<Productos> GetProductos()
    {
        string queryString = "SELECT idProducto, Descripcion, Precio FROM Productos";
        List<Productos> productos = new List<Productos>();
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Productos producto = new Productos
                    {
                        IdProducto = reader.GetInt32(0),               // Columna 0: idProducto
                        Descripcion = reader.GetString(1),              // Columna 1: Descripcion
                        Precio = reader.GetDecimal(2)
                    };                  // Columna 2: Precio

                    productos.Add(producto);
                }
            }
            connection.Close();
        }

        return productos;
    }

    public Productos GetProducto(int idProducto)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @idProducto";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@idProducto", idProducto));

        using var lector = comando.ExecuteReader();

        if (lector.Read())
        {
            var producto = new Productos
            {
                IdProducto = lector.GetInt32(0),
                Descripcion = lector.GetString(1),
                Precio = lector.GetDecimal(2)
            };

            return producto;
        }

        return null;
    }

    public void insertarProducto(Productos productoNuevo)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "INSERT INTO Productos (idProducto, Descripcion, Precio) VALUES (@idProducto, @Descripcion, @Precio)";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idProducto", productoNuevo.IdProducto));
        comando.Parameters.Add(new SqliteParameter("@Descripcion", productoNuevo.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", productoNuevo.Precio));

        comando.ExecuteNonQuery();
    }

    public void eliminarProducto(int idEliminar)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "DELETE FROM Productos WHERE idProducto = @idEliminar";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idProducto", idEliminar));
        comando.ExecuteNonQuery();
    }

    public void ActualizarProducto(int idProducto, decimal nuevoPrecio)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "UPDATE Productos SET Precio = @Precio WHERE idProducto = @idProducto";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@Precio", nuevoPrecio));
        comando.Parameters.Add(new SqliteParameter("@idProducto", idProducto));

        comando.ExecuteNonQuery();
    }


}