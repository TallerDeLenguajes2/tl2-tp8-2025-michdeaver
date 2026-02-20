using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using tl2_tp8_2025_michdeaver.Models;
using tl2_tp8_2025_michdeaver.Interfaces;

namespace tl2_tp8_2025_michdeaver.Repositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private readonly string connectionString;

        public PresupuestoRepository(string _connectionString)
        {
            connectionString = _connectionString;
        }

        public List<Presupuesto> GetPresupuestos()
        {
            string queryString = "SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";
            List<Presupuesto> presupuestos = new List<Presupuesto>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryString, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var presupuesto = new Presupuesto
                        {
                            IdPresupuesto = reader.GetInt32(0),
                            NombreDestinatario = reader.GetString(1),
                            // SQLite often stores dates as text; keep consistent:
                            FechaCreacion = DateTime.Parse(reader.GetString(2))
                        };

                        presupuestos.Add(presupuesto);
                    }
                }
            }

            return presupuestos;
        }

        public Presupuesto GetPresupuesto(int id)
        {
            string queryString = @"SELECT IdPresupuesto, NombreDestinatario, FechaCreacion
                                   FROM Presupuestos
                                   WHERE IdPresupuesto = @idPresupuesto";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@idPresupuesto", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var presupuesto = new Presupuesto
                            {
                                IdPresupuesto = reader.GetInt32(0),
                                NombreDestinatario = reader.GetString(1),
                                FechaCreacion = DateTime.Parse(reader.GetString(2))
                            };

                            // detalles uses SAME open connection (no extra Open inside)
                            presupuesto.Detalles = GetDetallePresupuesto(id, connection);

                            return presupuesto;
                        }
                    }
                }
            }

            throw new Exception("Presupuesto inexistente");
        }

        public void CreatePresupuesto(Presupuesto newPresupuesto)
        {
            string queryString = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion)
                                   VALUES (@nombreDestinatario, @fechaCreacion)";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@nombreDestinatario", newPresupuesto.NombreDestinatario);
                    command.Parameters.AddWithValue("@fechaCreacion", newPresupuesto.FechaCreacion.ToString("yyyy-MM-dd"));

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddDetallePresupuesto(int idPresupuesto, int idProducto, int cantidad)
        {
            string queryString = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
                                   VALUES (@idPresupuesto, @idProducto, @Cantidad)";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                    command.Parameters.AddWithValue("@idProducto", idProducto);
                    command.Parameters.AddWithValue("@Cantidad", cantidad);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeletePresupuesto(int id)
        {
            string queryString = "DELETE FROM Presupuestos WHERE IdPresupuesto = @idPresupuesto";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@idPresupuesto", id);

                    int filas = command.ExecuteNonQuery();

                    if (filas == 0)
                        throw new Exception("Presupuesto inexistente");
                }
            }
        }

        public void EditPresupuesto(int id, Presupuesto newPresupuesto)
        {
            string queryString = @"UPDATE Presupuestos
                                   SET NombreDestinatario = @NombreDestinatario,
                                       FechaCreacion = @FechaCreacion
                                   WHERE IdPresupuesto = @IdPresupuesto";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(queryString, connection))
                {
                    // Use the id parameter as the WHERE target (more consistent)
                    command.Parameters.AddWithValue("@IdPresupuesto", id);
                    command.Parameters.AddWithValue("@NombreDestinatario", newPresupuesto.NombreDestinatario);
                    command.Parameters.AddWithValue("@FechaCreacion", newPresupuesto.FechaCreacion.ToString("yyyy-MM-dd"));

                    int filas = command.ExecuteNonQuery();

                    if (filas == 0)
                        throw new Exception("Presupuesto inexistente");
                }
            }
        }

        // metodo auxiliar (NO abre la conexión: ya viene abierta)
        private List<PresupuestoDetalle> GetDetallePresupuesto(int id, SqliteConnection connection)
        {
            string queryString = @"SELECT p.idProducto, d.Cantidad, p.Descripcion, p.Precio
                                   FROM PresupuestosDetalle d
                                   INNER JOIN Productos p ON d.idProducto = p.idProducto
                                   WHERE d.idPresupuesto = @idPresupuesto";

            var detalles = new List<PresupuestoDetalle>();

            using (var command = new SqliteCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@idPresupuesto", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detalles.Add(new PresupuestoDetalle
                        {
                            Cantidad = reader.GetInt32(1),
                            Producto = new Producto
                            {
                                IdProducto = reader.GetInt32(0),
                                Descripcion = reader.GetString(2),
                                Precio = reader.GetInt32(3)
                            }
                        });
                    }
                }
            }

            return detalles;
        }
    }
}