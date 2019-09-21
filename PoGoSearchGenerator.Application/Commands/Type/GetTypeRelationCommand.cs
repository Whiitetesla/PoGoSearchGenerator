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
    public class GetTypeRelationCommand : IRequest<DamageRelation>
    {
        public string Type { get; set; }

        public GetTypeRelationCommand(string type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }

    public class GetTypeRelationCommandHandler
        : IRequestHandler<GetTypeRelationCommand, DamageRelation>
    {
        private readonly ApplicationDbContext _context;

        public GetTypeRelationCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DamageRelation> Handle(GetTypeRelationCommand request, CancellationToken cancellationToken)
        {
            if (!_context.Set<DamageRelation>().Any(x => x.Type == request.Type))
                await new PokeApiTypeDamageRelations(_context).GatherGetDamageRelation(request.Type);

            return _context.Set<DamageRelation>().Where(x => x.Type == request.Type).FirstOrDefault();
        }
    }
}
