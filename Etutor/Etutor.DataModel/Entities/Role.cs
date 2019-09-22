using Microsoft.AspNetCore.Identity;
using System;
using Etutor.Core;

namespace Etutor.DataModel.Entities
{
    public partial class Rol : IdentityRole<int>, IEntityAuditableBase
    {
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
