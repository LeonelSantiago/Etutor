using Etutor.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etutor.DataModel.Entities
{
    public class Events : BaseEntity
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
    }
}
