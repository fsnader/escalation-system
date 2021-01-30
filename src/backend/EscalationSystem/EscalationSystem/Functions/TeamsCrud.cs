using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EscalationSystem.Repository;
using EscalationSystem.Domain;
using System.Threading;
using EscalationSystem.Utils;

namespace EscalationSystem.Functions
{
    public class TeamsCrud
    {
        private readonly IRepository<Team> _teamRepository;

        public TeamsCrud(
            IRepository<Team> teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [FunctionName("CreateOrUpdateTeam")]
        public async Task<IActionResult> CreateOrUpdateTeamAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var team = await req.DeserializeBodyAsync<Team>();

            var updatedTeam = await _teamRepository.CreateOrUpdateAsync(team, cancellationToken);

            return new OkObjectResult(updatedTeam);
        }

        [FunctionName("GetTeam")]
        public async Task<IActionResult> GetTeamAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            Guid teamId = Guid.Parse(req.Query["id"]);

            var team = await _teamRepository.GetByIdAsync(teamId, cancellationToken);

            return new OkObjectResult(team);
        }

        [FunctionName("ListAllTeams")]
        public async Task<IActionResult> ListAllTeamsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var teams = await _teamRepository.ListAllAsync(cancellationToken);

            return new OkObjectResult(teams);
        }
    }
}
