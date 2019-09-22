using System;

namespace Etutor.Core
{
    public interface IEntityAuditableBase : IEntityBase
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset? ModifiedAt { get; set; }
        int CreatedBy { get; set; }
        int? ModifiedBy { get; set; }
        int Status { get; set; }

    }
}
