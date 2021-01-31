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

namespace EscalationSystem.Functions
{
    public class EmployeesCrud
    {
        private readonly IRepository<Employee> _repository;

        public EmployeesCrud(IRepository<Employee> EmployeeRepository)
        {
            _repository = EmployeeRepository;
        }

        [FunctionName("CreateOrUpdateEmployee")]
        public async Task<IActionResult> CreateOrUpdateEmployeeAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var employee = await req.DeserializeBodyAsync<Employee>();

            var updatedEmployee = await _repository.CreateOrUpdateAsync(employee, cancellationToken);

            return new OkObjectResult(updatedEmployee);
        }

        [FunctionName("GetEmployee")]
        public async Task<IActionResult> GetEmployeeAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            Guid employeeId = Guid.Parse(req.Query["id"]);

            var employee = await _repository.GetByIdAsync(employeeId, cancellationToken);

            return new OkObjectResult(employee);
        }

        [FunctionName("ListAllEmployees")]
        public async Task<IActionResult> ListAllEmployeesAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var employees = await _repository.ListAllAsync(cancellationToken);

            return new OkObjectResult(employees);
        }

        [FunctionName("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployeeAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            Guid employeeId = Guid.Parse(req.Query["id"]);

            await _repository.DeleteByIdAsync(employeeId, cancellationToken);

            return new OkObjectResult($"{employeeId} Employee deleted.");
        }
    }
}
