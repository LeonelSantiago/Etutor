using Microsoft.AspNetCore.Identity;
using System;
using Etutor.Core;

namespace Etutor.DataModel.Entities
{
    public partial class User : IdentityUser<int>, IEntityAuditableBase
    {
        #region Ctor
        public User()
        {

        }
        #endregion

        #region Properties
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        public DateTime? LastAccess { get; set; }
        public string ImgUrl { get; set; }

        #endregion

        #region Campos auditables
        /// <summary>
        /// Propiedad de cadena que representa el estado.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Propiedad boleana que representa si la entidad fue eliminada por el usuario.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Propiedad de fecha que representa la fecha en que fue creada la entidad.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Propiedad de fecha que representa la última fecha en que fue modificada la entidad.
        /// </summary>
        public DateTimeOffset? ModifiedAt { get; set; }

        /// <summary>
        /// Propiedad int que representa al usuario que creó la entidad.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Propiedad int que representa al último usuario que modificó la entidad.
        /// </summary>
        public int? ModifiedBy { get; set; }
        #endregion
    }
}
