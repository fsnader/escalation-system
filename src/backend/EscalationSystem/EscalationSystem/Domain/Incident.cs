using EscalationSystem.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscalationSystem.Domain
{
    public class Incident : IdentifiedEntity
    {
        public Guid Id { get; set; }
        public string TaylorId { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Description { get; set; }
        public Guid TeamId { get; set; }
        public Employee IncidentOwner { get; set; }
        public List<Event> Events { get; set; }
        public IncidentStatus Status { get; set; }
    }
}
