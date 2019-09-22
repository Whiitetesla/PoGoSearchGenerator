using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoGoSearchGenerator.Application.Commands.Type;
using PoGoSearchGenerator.Domain.Dto;

namespace PoGoSearchGeneratorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        /// <summary>
        /// <see cref="IMediator"/> to use.
        /// </summary>
        private readonly IMediator _mediator;

        public TypesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var result = await _mediator.Send(new GetTypesCommand());

            if (result == null || !result.Any())
                return new NotFoundResult();

            return new OkObjectResult(result);
        }

        [HttpGet("{type}")]
        public async Task<ActionResult<string>> GetAsync(string type)
        {
            var result = await _mediator.Send(new GetTypeRelationCommand(type));

            if (result == null)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<string>> PostAsync([FromBody] TypeCounterDto value)
        {
            return await _mediator.Send(new GetTypeCounterStringCommand(value));
        }
    }
}