using Etutor.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etutor.DataModel.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Status Status { get; set; }
    }
}
