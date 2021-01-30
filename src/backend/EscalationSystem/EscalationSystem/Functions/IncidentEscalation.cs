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

namespace EscalationSystem.Functions
{
    public class IncidentEscalation
    {
        private readonly IRepository<Team> _repository;
        private readonly IRepository<Incident> _incidentRepository;
        private const int MaxRetry = 3;

        public IncidentEscalation(
            IRepository<Team> repository,
            IRepository<Incident> incidentRepository)
        {
            _repository = repository;
            _incidentRepository = incidentRepository;
        }

        [FunctionName("EscalateIncident")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger logger)
        {
            var incident = await req.DeserializeBodyAsync<Incident>();

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

            foreach (var employee in team.Employees)
            {
                for (int currentRetry = 0; currentRetry < MaxRetry; currentRetry++)
                {
                    var status = await context.CallActivityAsync<CallStatus>("CallEmployee", employee);

                    incident.Events.Add(new Event
                    {
                        Id = Guid.NewGuid(),
                        Employee = employee.Name,
                        DateTime = context.CurrentUtcDateTime,
                        Status = status
                    });

                    if (status == CallStatus.Answered)
                    {
                        return;
                    }
                }

                await context.CreateTimer(context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(5)), CancellationToken.None);
            }

            if (team.NextTeamId != null)
            {
                incident.TeamId = team.NextTeamId;
                string instanceId = await starter.StartNewAsync("EscalationOrchestrator", null, incident);
            }
            else
            {
                var status = await context.CallActivityAsync<CallStatus>("CallEmployee", incident.IncidentOwner);

                if (status != CallStatus.Answered)
                {
                    await context.CallActivityAsync("SendEmailToEmployee", incident.IncidentOwner);
                }
            }
        }

        [FunctionName("UpdateIncident")]
        public async Task UpdateIncident([ActivityTrigger] Incident incident, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogWarning("Updating incident {id}", incident.Id);
            await _incidentRepository.CreateOrUpdateAsync(incident, cancellationToken);
        }

        [FunctionName("GetIncidentTeam")]
        public async Task<Team> GetIncidentTeam([ActivityTrigger] Guid teamId, ILogger logger, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(teamId, cancellationToken);
        }

        [FunctionName("CallEmployee")]
        public async Task<CallStatus> CallEmployee([ActivityTrigger] Employee employee, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogWarning("Calling employee {name}", employee.Name);
            // Colocar o pooling aqui para aguardar a mudança de status da ligação
            return CallStatus.Answered;
        }

        [FunctionName("SendEmailToEmployee")]
        public async Task SendEmailToEmployee([ActivityTrigger] Employee employee, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogError("Sending e-mail to employee {name}", employee.Name);
        }
    }
}
