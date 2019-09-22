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
                    new User { Name = "Manuel Vladimir", LastName = "Oleaga Begaso", Email = "manuel.oleaga@gmail.com", UserName = "manuel.oleaga", LockoutEnabled = false },
                    new User { Name = "Jose Rafael", LastName = "Aquino Balbuena", Email = "jose.aquino@gmail.com", UserName = "jose.aquino", LockoutEnabled = false },
                    new User { Name = "Octavio", LastName = "Suero", Email = "octavio.augusto@gmail.com", UserName = "octavio.augusto", LockoutEnabled = false },
                    new User { Name = "Leonel", LastName = "Santiago", Email = "leonel.santiago@gmail.com", UserName = "leonel.santiago", LockoutEnabled = false },
                    new User { Name = "Ady", LastName = "Taveras", Email = "ady.taveras@gmail.com", UserName = "ady.taveras", LockoutEnabled = false },
                    new User { Name = "Christian", LastName = "Marinelli", Email = "christian.marinelli@gmail.com", UserName = "christian.marinelli", LockoutEnabled = false },
                    new User { Name = "Daniel", LastName = "Mejía", Email = "daniel.mejia@gmail.com", UserName = "daniel.mejia", LockoutEnabled = false },
                    new User { Name = "Danilo", LastName = "Duarte", Email = "danilo.duarte@gmail.com", UserName = "danilo.duarte", LockoutEnabled = false },
                    new User { Name = "Adriana", LastName = "Adriana Henríquez", Email = "adriana.henriquez@gmail.com", UserName = "adriana.henriquez", LockoutEnabled = false },
                    new User { Name = "Aida", LastName = "Aida Sánchez", Email = "aida.sanchez@gmail.com", UserName = "aida.sanchez", LockoutEnabled = false },
                    new User { Name = "Ambar", LastName = "Quiñonez", Email = "aquinonez@adess.gob.do", UserName = "aquinonez", LockoutEnabled = false },
                    new User { Name = "Amanda Tatiana", LastName = "Forteza Collado", Email = "amanda.forteza@gmail.com", UserName = "amanda.forteza", LockoutEnabled = false },
                    new User { Name = "Fernando", LastName = "Medina", Email = "fernando.medina@gmail.com", UserName = "fernando.medina", LockoutEnabled = false },
                    new User { Name = "Fiordaliza", LastName = "Mateo Landa", Email = "Fiordaliza.Mateo@gmail.com", UserName = "Fiordaliza.Mateo", LockoutEnabled = false },
                    new User { Name = "Francisco", LastName = "Florencio Solis", Email = "Francisco.Florencio@gmail.com", UserName = "Francisco.Florencio", LockoutEnabled = false },
                    new User { Name = "Francys", LastName = "Rosario", Email = "francys.rosario@gmail.com", UserName = "francys.rosari", LockoutEnabled = false },
                    new User { Name = "Francisco", LastName = "Caceres Ureña", Email = "Francisco.Caceres@gmail.com", UserName = "Francisco.Caceres", LockoutEnabled = false },
                    new User { Name = "Freddy", LastName = "Perez", Email = "Freddy.Perez@gmail.com", UserName = "Freddy.Perez", LockoutEnabled = false },
                    new User { Name = "Gabriela", LastName = "Ferreiras", Email = "gabriela.ferreiras@gmail.com", UserName = "gabriela.ferreiras", LockoutEnabled = false },
                    new User { Name = "Hirma Isabel", LastName = "Aponte Chapman", Email = "hirma.aponte@gmail.com", UserName = "hirma.aponte", LockoutEnabled = false },
                    new User { Name = "Jeisy", LastName = "Fermin", Email = "Jeisy.Fermin@gmail.com", UserName = "", LockoutEnabled = false },
                    new User { Name = "Jessanin Diosmery", LastName = "Frias Peña", Email = "Jessanin.Frias@gmail.com", UserName = "Jessanin.Frias", LockoutEnabled = false },
                    new User { Name = "José Anibal", LastName = "Jiménez Guillén", Email = "jose.jimenez@gmail.com", UserName = "jose.jimenez", LockoutEnabled = false },
                    new User { Name = "Mario EmiliO", LastName = "Fernandez Cepeda", Email = "mario.fernandez@gmail.com", UserName = "mario.fernandez", LockoutEnabled = false },
                    new User { Name = "Niurka", LastName = "Figuereo", Email = "Niurka.Figuereo@gmail.com", UserName = "Niurka.Figuereo", LockoutEnabled = false },
                    new User { Name = "Rikelvi", LastName = "Fermin", Email = "rikelvi.fermin@gmail.com", UserName = "rikelvi.fermin", LockoutEnabled = false },
                    new User { Name = "Robin", LastName = "Ferreira", Email = "Robin.Ferreira@gmail.com", UserName = "Robin.Ferreira", LockoutEnabled = false },
                    new User { Name = "Santiago", LastName = "Farjat Bascón", Email = "sfarjat@ine.gob.bo", UserName = "sfarjat", LockoutEnabled = false },
                    new User { Name = "Ariel", LastName = "Fermin", Email = "Ariel.Fermin@gmail.com", UserName = "Ariel.Fermin", LockoutEnabled = false },
                    new User { Name = "Domingo", LastName = "Fenton", Email = "Domingo.Fenton@gmail.com", UserName = "Domingo.Fenton", LockoutEnabled = false },
                    new User { Name = "Darwin ErnestO", LastName = "Florentino Beato", Email = "Darwin.Florentin@gmail.como", UserName = "Darwin.Florentin", LockoutEnabled = false },
                    new User { Name = "Florangel", LastName = "Mora Arias", Email = "Florangel.Mora@gmail.com", UserName = "Florangel.Mora", LockoutEnabled = false },
                    new User { Name = "Francis del Carmen", LastName = "Abraham Almanzar", Email = "Francis.Abraham@gmail.com", UserName = "Francis.Abraham", LockoutEnabled = false }
                };
            }
        }

        public static List<User> Usuarios { get => _usuario; set => _usuario = value; }
    }
}
