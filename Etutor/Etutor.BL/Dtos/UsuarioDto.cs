using Etutor.BL.Abstract;

namespace Etutor.BL.Dtos
{
    public class UsuarioDto : IEntityBaseDto
    {
        #region Ctor
        public UsuarioDto()
        {

        }
        #endregion

        #region Properties
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contrasena { get; set; }

        /*Identity fields*/
        public virtual string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        /*End Identity fields*/
        #endregion
    }
}
