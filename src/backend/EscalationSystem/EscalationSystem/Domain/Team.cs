using System;
using System.Collections.Generic;
using System.Text;

namespace EscalationSystem.Domain
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
        public Guid NextTeamId { get; set; }
    }
}
