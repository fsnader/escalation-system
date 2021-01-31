using EscalationSystem.Repository;
using System;

namespace EscalationSystem.Domain
{
    public class Event : IdentifiedEntity
    {
        public Event() { }

        public Event(string employee, DateTimeOffset datetime, CallStatus status)
        { 
            Id = Guid.NewGuid();
            Employee = employee;
            DateTime = datetime;
            Status = status;
        }

        public Guid Id { get; set; }
        public string Employee { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public CallStatus Status { get; set; }
    }
}
