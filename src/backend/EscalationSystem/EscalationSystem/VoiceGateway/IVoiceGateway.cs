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
        public Task<string> CallNumberAsync(string phoneNumber, string name, CancellationToken cancellationToken);

        public Task<CallStatus> GetCallStatusAsync(string messageId, string phoneNumber, CancellationToken cancellationToken);
    }
}
