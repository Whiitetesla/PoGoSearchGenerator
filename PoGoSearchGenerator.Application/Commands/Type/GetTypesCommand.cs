using MediatR;
using PoGoSearchGenerator.Domain.Entities;
using PoGoSearchGenerator.infrastructure.Efcore;
using PoGoSearchGenerator.infrastructure.PokeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGoSearchGenerator.Application.Commands.Type
{
    public class GetTypesCommand : IRequest<List<Types>>
    {
    }

    public class GetTypesCommandHandler
        : IRequestHandler<GetTypesCommand, List<Types>>
    {
        private readonly ApplicationDbContext _context;

        public GetTypesCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Types>> Handle(GetTypesCommand request, CancellationToken cancellationToken)
        {
            if (!_context.Set<Types>().Any())
                await new PokeApiTypeGather(_context).GatherTypeListAsync();

            return _context.Set<Types>().ToList();
        }
    }
}
