using Etutor.Core;
using Etutor.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etutor.DataModel.Entities
{
    public class Events : IEntityAuditableBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventTypeId { get; set; }
        public virtual EventsTypes EventsTypes { get; set; }

        #region Auditable properties
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int Status { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        #endregion Auditable properties

    }
}
