using MediatR;
using Microsoft.AspNetCore.Mvc;
using PoGoSearchGenerator.Application.Commands.Type;
using PoGoSearchGenerator.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostAsync([FromBody] TypeCounterDto value)
        {
            return new OkObjectResult(await _mediator.Send(new GetTypeCounterStringCommand(value)));
        }
    }
}