using EscalationSystem.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EscalationSystem.VoiceGateway
{
    public interface IVoiceGateway
    {
        public Task<CallStatus> CallNumberAsync(string phoneNumber, string name, CancellationToken cancellationToken);
    }
}
