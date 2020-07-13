using Etutor.BL.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etutor.BL.Dtos
{
    public class EventsDto : IEntityBaseDto
    {
        #region Ctor
        public EventsDto()
        {

        }

        #endregion
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventTypeId { get; set; }
        public virtual EventsDto EventsTypes { get; set; }
    }
}
