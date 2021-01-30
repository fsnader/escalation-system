using System;
using System.Collections.Generic;
using System.Text;

namespace EscalationSystem.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string SapId { get; set; }

        public string Name { get; set; }

        public string Cellphone {get;set;}

        public string Email { get; set; }
    }
}
