using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;

using tl2_tp8_2025_michdeaver.Models;
using tl2_tp8_2025_michdeaver.Interfaces;

namespace tl2_tp8_2025_michdeaver.Repositories
{
    public class UserRepository : IUserRepository
    {
         private readonly string connectionString;

        public UserRepository(string _connectionString)
        {
            connectionString = _connectionString;
        }
        public User GetUser(string username, string password)
        {
            string queryString = @"SELECT idUsuario, Nombre, Username, Password, Rol FROM Usuarios WHERE Username = @Username AND Password = @Password";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = new SqliteCommand(queryString, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new User
                        {
                            IdUser = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Username = reader.GetString(2),
                            Password = reader.GetString(3),
                            Rol = reader.GetString(4)
                        };
                        return user;
                    }
                }
                
                return null;
            }
        }
    }
}