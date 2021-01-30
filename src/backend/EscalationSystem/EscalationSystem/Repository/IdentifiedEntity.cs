using System;
using System.Collections.Generic;
using System.Text;

namespace EscalationSystem.Repository
{
    public interface IdentifiedEntity
    {
        public Guid Id { get; set; }
    }
}
