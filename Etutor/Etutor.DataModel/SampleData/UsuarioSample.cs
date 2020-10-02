using Etutor.DataModel.Entities;
using System.Collections.Generic;

namespace Etutor.DataModel.SampleData
{
    public class UsuarioSample
    {
        private static List<User> _usuario;

        static UsuarioSample()
        {
            if (_usuario == null)
            {
                Usuarios = new List<User>()
                {
                    new User { Name = "Leonel", LastName = "Santiago", Email = "leosantiagobaez@gmail.com", UserName = "leonel.santiago", LockoutEnabled = false },
                    new User { Name = "Wilson", LastName = "Reyes", Email = "wilson.reyes@gmail.com", UserName = "wilson.reyes", LockoutEnabled = false }               
                };
            }
        }

        public static List<User> Usuarios { get => _usuario; set => _usuario = value; }
    }
}
