using Etutor.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etutor.DataModel.Entities
{
    public class EventsTypes : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsGlobal { get; set; }
    }
}
