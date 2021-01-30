using EscalationSystem;
using EscalationSystem.Domain;
using EscalationSystem.Repository;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace EscalationSystem
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddSingleton<IRepository<Team>, MongoRepository<Team>>();
            builder.Services.AddSingleton<IRepository<Incident>, MongoRepository<Incident>>();
        }
    }
}
