using EscalationSystem.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscalationSystem.Domain
{
    public class Event : IdentifiedEntity
    {
        public Guid Id { get; set; }
        public string Employee { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public CallStatus Status { get; set; }
    }
}
