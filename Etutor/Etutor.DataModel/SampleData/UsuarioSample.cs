using Etutor.DataModel.Entities;
using System.Collections.Generic;

namespace Etutor.DataModel.SampleData
{
    public class UsuarioSample
    {
        private static List<Usuario> _usuario;

        static UsuarioSample()
        {
            if (_usuario == null)
            {
                Usuarios = new List<Usuario>()
                {
                    new Usuario { Nombre = "Manuel Vladimir", Apellido = "Oleaga Begaso", Email = "manuel.oleaga@one.gob.do", UserName = "manuel.oleaga", LockoutEnabled = false },
                    new Usuario { Nombre = "Jose Rafael", Apellido = "Aquino Balbuena", Email = "jose.aquino@one.gob.do", UserName = "jose.aquino", LockoutEnabled = false },
                    new Usuario { Nombre = "Octavio", Apellido = "Suero", Email = "octavio.augusto@one.gob.do", UserName = "octavio.augusto", LockoutEnabled = false },
                    new Usuario { Nombre = "Leonel", Apellido = "Santiago", Email = "leonel.santiago@one.gob.do", UserName = "leonel.santiago", LockoutEnabled = false },
                    new Usuario { Nombre = "Ady", Apellido = "Taveras", Email = "ady.taveras@one.gob.do", UserName = "ady.taveras", LockoutEnabled = false },
                    new Usuario { Nombre = "Christian", Apellido = "Marinelli", Email = "christian.marinelli@one.gob.do", UserName = "christian.marinelli", LockoutEnabled = false },
                    new Usuario { Nombre = "Daniel", Apellido = "Mejía", Email = "daniel.mejia@one.gob.do", UserName = "daniel.mejia", LockoutEnabled = false },
                    new Usuario { Nombre = "Danilo", Apellido = "Duarte", Email = "danilo.duarte@one.gob.do", UserName = "danilo.duarte", LockoutEnabled = false },
                    new Usuario { Nombre = "Adriana", Apellido = "Adriana Henríquez", Email = "adriana.henriquez@one.gob.do", UserName = "adriana.henriquez", LockoutEnabled = false },
                    new Usuario { Nombre = "Aida", Apellido = "Aida Sánchez", Email = "aida.sanchez@one.gob.do", UserName = "aida.sanchez", LockoutEnabled = false },
                    new Usuario { Nombre = "Ambar", Apellido = "Quiñonez", Email = "aquinonez@adess.gob.do", UserName = "aquinonez", LockoutEnabled = false },
                    new Usuario { Nombre = "Amanda Tatiana", Apellido = "Forteza Collado", Email = "amanda.forteza@one.gob.do", UserName = "amanda.forteza", LockoutEnabled = false },
                    new Usuario { Nombre = "Fernando", Apellido = "Medina", Email = "fernando.medina@one.gob.do", UserName = "fernando.medina", LockoutEnabled = false },
                    new Usuario { Nombre = "Fiordaliza", Apellido = "Mateo Landa", Email = "Fiordaliza.Mateo@one.gob.do", UserName = "Fiordaliza.Mateo", LockoutEnabled = false },
                    new Usuario { Nombre = "Francisco", Apellido = "Florencio Solis", Email = "Francisco.Florencio@one.gob.do", UserName = "Francisco.Florencio", LockoutEnabled = false },
                    new Usuario { Nombre = "Francys", Apellido = "Rosario", Email = "francys.rosario@one.gob.do", UserName = "francys.rosari", LockoutEnabled = false },
                    new Usuario { Nombre = "Francisco", Apellido = "Caceres Ureña", Email = "Francisco.Caceres@one.gob.do", UserName = "Francisco.Caceres", LockoutEnabled = false },
                    new Usuario { Nombre = "Freddy", Apellido = "Perez", Email = "Freddy.Perez@one.gob.do", UserName = "Freddy.Perez", LockoutEnabled = false },
                    new Usuario { Nombre = "Gabriela", Apellido = "Ferreiras", Email = "gabriela.ferreiras@one.gob.do", UserName = "gabriela.ferreiras", LockoutEnabled = false },
                    new Usuario { Nombre = "Hirma Isabel", Apellido = "Aponte Chapman", Email = "hirma.aponte@one.gob.do", UserName = "hirma.aponte", LockoutEnabled = false },
                    new Usuario { Nombre = "Jeisy", Apellido = "Fermin", Email = "Jeisy.Fermin@one.gob.do", UserName = "", LockoutEnabled = false },
                    new Usuario { Nombre = "Jessanin Diosmery", Apellido = "Frias Peña", Email = "Jessanin.Frias@one.gob.do", UserName = "Jessanin.Frias", LockoutEnabled = false },
                    new Usuario { Nombre = "José Anibal", Apellido = "Jiménez Guillén", Email = "jose.jimenez@one.gob.do", UserName = "jose.jimenez", LockoutEnabled = false },
                    new Usuario { Nombre = "Mario EmiliO", Apellido = "Fernandez Cepeda", Email = "mario.fernandez@one.gob.do", UserName = "mario.fernandez", LockoutEnabled = false },
                    new Usuario { Nombre = "Niurka", Apellido = "Figuereo", Email = "Niurka.Figuereo@one.gob.do", UserName = "Niurka.Figuereo", LockoutEnabled = false },
                    new Usuario { Nombre = "Rikelvi", Apellido = "Fermin", Email = "rikelvi.fermin@one.gob.do", UserName = "rikelvi.fermin", LockoutEnabled = false },
                    new Usuario { Nombre = "Robin", Apellido = "Ferreira", Email = "Robin.Ferreira@one.gob.do", UserName = "Robin.Ferreira", LockoutEnabled = false },
                    new Usuario { Nombre = "Santiago", Apellido = "Farjat Bascón", Email = "sfarjat@ine.gob.bo", UserName = "sfarjat", LockoutEnabled = false },
                    new Usuario { Nombre = "Ariel", Apellido = "Fermin", Email = "Ariel.Fermin@one.gob.do", UserName = "Ariel.Fermin", LockoutEnabled = false },
                    new Usuario { Nombre = "Domingo", Apellido = "Fenton", Email = "Domingo.Fenton@one.gob.do", UserName = "Domingo.Fenton", LockoutEnabled = false },
                    new Usuario { Nombre = "Darwin ErnestO", Apellido = "Florentino Beato", Email = "Darwin.Florentin@one.gob.doo", UserName = "Darwin.Florentin", LockoutEnabled = false },
                    new Usuario { Nombre = "Florangel", Apellido = "Mora Arias", Email = "Florangel.Mora@one.gob.do", UserName = "Florangel.Mora", LockoutEnabled = false },
                    new Usuario { Nombre = "Francis del Carmen", Apellido = "Abraham Almanzar", Email = "Francis.Abraham@one.gob.do", UserName = "Francis.Abraham", LockoutEnabled = false }
                };
            }
        }

        public static List<Usuario> Usuarios { get => _usuario; set => _usuario = value; }
    }
}
