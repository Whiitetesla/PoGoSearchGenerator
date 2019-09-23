using MediatR;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// <see cref="ApplicationDbContext"/> to use
        /// </summary>
        private readonly ApplicationDbContext _context;

        public GetTypeRelationCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DamageRelation> Handle(GetTypeRelationCommand request, CancellationToken cancellationToken)
        {
            //check if we have a damageRelation with the type we search for
            if (!_context.Set<DamageRelation>().Any(x => x.Type == request.Type))
            {
                //if not we all api for the information
                if(!await new PokeApiTypeDamageRelations(_context).GatherGetDamageRelation(request.Type))
                    return null;
            }

            //return damageRelation from db and include all it's lists
            return _context.Set<DamageRelation>()
                .Include(x => x.Double_damage_from)
                .Include(x => x.Double_damage_to)
                .Include(x => x.Half_damage_from)
                .Include(x => x.No_damage_from)
                .Where(x => x.Type == request.Type)
                .FirstOrDefault();
        }
    }
}
