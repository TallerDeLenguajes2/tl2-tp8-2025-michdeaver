using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tl2_tp8_2025_michdeaver.Models
{
    public class User
    {
        private int idUser;
        private string nombre;
        private string username;
        private string password;
        private string rol;

        public User(int idUser, string nombre, string username, string password, string rol)
        {
            this.idUser = idUser;
            this.nombre = nombre;
            this.username = username;
            this.password = password;
            this.rol = rol;
        }

        public int IdUser { get => idUser; set => idUser = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Rol { get => rol; set => rol = value; }
    }
}