using Microsoft.Data.Sqlite;

public class PresupuestosRepository
{
    string connectionString = "Data Source=db/Tienda.db";

    public List<Presupuestos> GetPresupuestos()
    {
        string queryString = "SELECT idPresupuesto, nombreDestinatario, FechaCreacion FROM Presupuestos";
        List<Presupuestos> presupuestos = new List<Presupuestos>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(queryString, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var presupuesto = new Presupuestos
                    {
                        IdPresupuesto = reader.GetInt32(0),
                        NombreDestinatario = reader.GetString(1),
                        FechaCreacion = reader.GetDateTime(2)
                    };
                    presupuestos.Add(presupuesto);
                }
            }
        }

        return presupuestos;
    }

    public Presupuestos GetPresupuesto(int idPresupuesto)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "SELECT idPresupuesto, nombreDestinatario, FechaCreacion FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));

        using var lector = comando.ExecuteReader();

        if (lector.Read())
        {
            var presupuesto = new Presupuestos
            {
                IdPresupuesto = lector.GetInt32(0),
                NombreDestinatario = lector.GetString(1),
                FechaCreacion = lector.GetDateTime(2)
            };

            var detalles = GetDetallesPorPresupuesto(idPresupuesto);

            presupuesto.Detalle = detalles;

            return presupuesto;
        }

        return null;
    }

    public void InsertarPresupuesto(Presupuestos nuevo)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "INSERT INTO Presupuestos (idPresupuesto, nombreDestinatario, FechaCreacion) VALUES (@idPresupuesto, @nombreDestinatario, @fechaCreacion)";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", nuevo.IdPresupuesto));
        comando.Parameters.Add(new SqliteParameter("@nombreDestinatario", nuevo.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@fechaCreacion", nuevo.FechaCreacion));

        comando.ExecuteNonQuery();
    }

    public void AgregarProductoAPresupuesto(int idPresupuesto, Productos producto, int cantidad)
    {
        // Verificar que el presupuesto exista
        var presupuesto = GetPresupuesto(idPresupuesto);
        if (presupuesto == null)
            throw new Exception($"No existe un presupuesto con el ID {idPresupuesto}");

        // Reutilizar el repositorio de detalle
        using var conexion = new SqliteConnection(connectionString);

        conexion.Open();

        string sql = @"INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, Cantidad)
                        VALUES (@idPresupuesto, @idProducto, @Cantidad)";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
        comando.Parameters.Add(new SqliteParameter("@idProducto", producto.IdProducto));
        comando.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

        comando.ExecuteNonQuery(); //para actualizar la BD
    }

    public void EliminarPresupuesto(int idEliminar)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idEliminar));
        comando.ExecuteNonQuery();
    }

    public void ActualizarPresupuesto(int idPresupuesto, string nuevoNombreDestinatario)
    {
        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "UPDATE Presupuestos SET nombreDestinatario = @nombreDestinatario WHERE idPresupuesto = @idPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@nombreDestinatario", nuevoNombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));

        comando.ExecuteNonQuery();
    }


    //METODOS PRIVADOS
    private List<PresupuestoDetalle> GetDetallesPorPresupuesto(int idPresupuesto)
    {
        List<PresupuestoDetalle> detalles = new List<PresupuestoDetalle>();

        using var conexion = new SqliteConnection(connectionString);
        conexion.Open();

        string sql = "SELECT idProducto, Cantidad FROM PresupuestoDetalle WHERE idPresupuesto = @idPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));

        using var lector = comando.ExecuteReader();

        var productoRepo = new ProductosRepository();

        while (lector.Read())
        {
            int idProducto = lector.GetInt32(1);
            int cantidad = lector.GetInt32(2);

            Productos producto = productoRepo.GetProducto(idProducto);

            var detalle = new PresupuestoDetalle
            {
                Producto = producto,
                Cantidad = cantidad
            };

            detalles.Add(detalle);
        }

        return detalles;
    }
}