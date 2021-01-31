using EscalationSystem.Domain;
using EscalationSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EscalationSystem.Functions
{
    public class IncidentsCrud
    {
        private readonly IRepository<Incident> _repository;

        public IncidentsCrud(IRepository<Incident> repository)
        {
            _repository = repository;
        }

        [FunctionName("GetIncident")]
        public async Task<IActionResult> GetIncidentAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            Guid incidentId = Guid.Parse(req.Query["id"]);
            var team = await _repository.GetByIdAsync(incidentId, cancellationToken);
            return new OkObjectResult(team);
        }

        [FunctionName("ListAllIncidents")]
        public async Task<IActionResult> ListAllTeamsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var teams = await _repository.ListAllAsync(cancellationToken);
            return new OkObjectResult(teams);
        }
    }
}
