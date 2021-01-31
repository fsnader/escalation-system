using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EscalationSystem.Domain;
using EscalationSystem.Utils;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using EscalationSystem.Repository;
using System.Threading;
using EscalationSystem.VoiceGateway;

namespace EscalationSystem.Functions
{
    public class IncidentEscalation
    {
        private readonly IRepository<Team> _repository;
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IVoiceGateway _voiceGateway;
        private const int MaxRetry = 3;

        private TimeSpan TimeBetweenCalls = TimeSpan.FromMinutes(5);

        private const int MaxStatusRetries = 3;
        private TimeSpan TimeBetweenStatusTries = TimeSpan.FromMinutes(1);

        public IncidentEscalation(
            IRepository<Team> repository,
            IRepository<Incident> incidentRepository,
            IVoiceGateway voiceGateway)
        {
            _repository = repository;
            _incidentRepository = incidentRepository;
            _voiceGateway = voiceGateway;
        }

        [FunctionName("EscalateIncident")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger logger)
        {
            var receivedIncident = await req.DeserializeBodyAsync<Incident>();

            var incident = await _incidentRepository.CreateOrUpdateAsync(receivedIncident, CancellationToken.None);
            string instanceId = await starter.StartNewAsync("EscalationOrchestrator", null, incident);
            logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("EscalationOrchestrator")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger logger)
        {
            var incident = context.GetInput<Incident>();

            var team = await context.CallActivityAsync<Team>("GetIncidentTeam", incident.TeamId);

            if (team != null)
            {
                foreach (var employee in team?.Employees)
                {
                    for (int currentRetry = 0; currentRetry < MaxRetry; currentRetry++)
                    {
                        await AddIncidentEventAsync(incident, CallStatus.Calling, employee.Name, context, logger);
                        
                        var messageId = await context.CallActivityAsync<string>("CallEmployee", employee);
                        CallStatus status = await GetCallStatus(context, employee, messageId);

                        await AddIncidentEventAsync(incident, status, employee.Name, context, logger);

                        if (status == CallStatus.Answered)
                        {
                            incident.Status = IncidentStatus.Closed;
                            await context.CallActivityAsync("UpdateIncident", incident);
                            return;
                        }

                        await context.CreateTimer(context.CurrentUtcDateTime.Add(TimeBetweenCalls), CancellationToken.None);
                    }
                }
            }

            if (team?.NextTeamId != null)
            {
                logger.LogError("Escalating incident {incidentId} to next team", incident.TaylorId);
                incident.TeamId = team.NextTeamId;
                await starter.StartNewAsync("EscalationOrchestrator", null, incident);
            }
            else
            {
                await AddIncidentEventAsync(incident, CallStatus.Calling, incident.IncidentOwner.Name, context, logger);

                var employeeCallMessageId = await context.CallActivityAsync<string>("CallEmployee", incident.IncidentOwner);
                var status = await GetCallStatus(context, incident.IncidentOwner, employeeCallMessageId);

                await AddIncidentEventAsync(incident, status, incident.IncidentOwner.Name, context, logger);

                if (status != CallStatus.Answered)
                {
                    await context.CallActivityAsync("SendEmailToEmployee", incident.IncidentOwner);
                    await AddIncidentEventAsync(incident, CallStatus.EmailSent, incident.IncidentOwner.Name, context, logger);
                }

                incident.Status = IncidentStatus.Cancelled;
                await context.CallActivityAsync("UpdateIncident", incident);
            }
        }

        private async Task<CallStatus> GetCallStatus(IDurableOrchestrationContext context, Employee employee, string messageId)
        {
            CallStatus status = CallStatus.Calling;

            for (int statusTry = 0; statusTry < MaxStatusRetries; statusTry++)
            {
                await context.CreateTimer(context.CurrentUtcDateTime.Add(TimeBetweenStatusTries), CancellationToken.None);
                status = await context.CallActivityAsync<CallStatus>("GetCallStatus", (messageId, employee.Cellphone));

                if (status != CallStatus.Calling) break;
            }

            if (status == CallStatus.Calling) status = CallStatus.Lost;
            return status;
        }

        private async Task AddIncidentEventAsync(Incident incident, CallStatus status, string employee, IDurableOrchestrationContext context, ILogger logger)
        {
            incident.Events.Add(new Event(employee, context.CurrentUtcDateTime, status));
            await context.CallActivityAsync("UpdateIncident", incident);
        }

        [FunctionName("UpdateIncident")]
        public async Task UpdateIncident([ActivityTrigger] Incident incident, ILogger logger, CancellationToken cancellationToken)
        {
            await _incidentRepository.CreateOrUpdateAsync(incident, cancellationToken);
        }

        [FunctionName("GetIncidentTeam")]
        public async Task<Team> GetIncidentTeam([ActivityTrigger] Guid teamId, ILogger logger, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(teamId, cancellationToken);
        }

        [FunctionName("CallEmployee")]
        public async Task<string> CallEmployee([ActivityTrigger] Employee employee, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogWarning("Calling employee {name}", employee.Name );
            var messageId = await _voiceGateway.CallNumberAsync(employee.Cellphone, employee.Name, cancellationToken);
            logger.LogWarning("Called employee {name} with messageId {messageId}", employee.Name, messageId);

            return messageId;
        }

        [FunctionName("GetCallStatus")]
        public async Task<CallStatus> GetCallStatus([ActivityTrigger] IDurableActivityContext inputs, ILogger logger, CancellationToken cancellationToken)
        {
            var (messageId, phoneNumber) = inputs.GetInput<(string, string)>();

            logger.LogWarning("Geting status of call id {messageId}", messageId);
            return await _voiceGateway.GetCallStatusAsync(messageId, phoneNumber, cancellationToken);
        }

        [FunctionName("SendEmailToEmployee")]
        public async Task SendEmailToEmployee([ActivityTrigger] Employee employee, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogError("Sending e-mail to employee {name}", employee.Name);
        }
    }
}
