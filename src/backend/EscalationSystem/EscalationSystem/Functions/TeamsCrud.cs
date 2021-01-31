using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EscalationSystem.Repository;
using EscalationSystem.Domain;
using System.Threading;
using EscalationSystem.Utils;
using EscalationSystem.VoiceGateway;

namespace EscalationSystem.Functions
{
    public class TeamsCrud
    {
        private readonly IRepository<Team> _repository;
        private readonly IVoiceGateway _voiceGateway;

        public TeamsCrud(IRepository<Team> teamRepository, IVoiceGateway voiceGateway)
        {
            _repository = teamRepository;
            _voiceGateway = voiceGateway;
        }

        [FunctionName("CreateOrUpdateTeam")]
        public async Task<IActionResult> CreateOrUpdateTeamAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var team = await req.DeserializeBodyAsync<Team>();

            var updatedTeam = await _repository.CreateOrUpdateAsync(team, cancellationToken);

            return new OkObjectResult(updatedTeam);
        }

        [FunctionName("GetTeam")]
        public async Task<IActionResult> GetTeamAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            Guid teamId = Guid.Parse(req.Query["id"]);

            var team = await _repository.GetByIdAsync(teamId, cancellationToken);

            return new OkObjectResult(team);
        }

        [FunctionName("ListAllTeams")]
        public async Task<IActionResult> ListAllTeamsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var teams = await _repository.ListAllAsync(cancellationToken);

            return new OkObjectResult(teams);
        }

        [FunctionName("DeleteTeam")]
        public async Task<IActionResult> DeleteTeamAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            Guid teamId = Guid.Parse(req.Query["id"]);

            await _repository.DeleteByIdAsync(teamId, cancellationToken);

            return new OkObjectResult($"{teamId} team deleted.");
        }

        [FunctionName("Call")]
        public async Task<IActionResult> CallAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            string phoneNumber = req.Query["phone"];
            string name = req.Query["name"];

            await _voiceGateway.CallNumberAsync(phoneNumber, name, cancellationToken);

            return new OkObjectResult($"{phoneNumber} called.");
        }
    }
}
