using Etutor.BL.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etutor.BL.Dtos
{
    public class EventsTypesDto : IEntityBaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsGlobal { get; set; }
    }
}
