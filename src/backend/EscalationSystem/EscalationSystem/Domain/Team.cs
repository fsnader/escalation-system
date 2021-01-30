using EscalationSystem.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscalationSystem.Domain
{
    public class Team : IdentifiedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
        public Guid NextTeamId { get; set; }
    }
}
