using System;

namespace Etutor.Core
{
    public interface IEntityAuditableBase : IEntityBase
    {
        DateTimeOffset FechaCreacion { get; set; }
        DateTimeOffset? FechaModificacion { get; set; }
        int CreadoPor { get; set; }
        int? EditadoPor { get; set; }
        string Estado { get; set; }

    }
}
