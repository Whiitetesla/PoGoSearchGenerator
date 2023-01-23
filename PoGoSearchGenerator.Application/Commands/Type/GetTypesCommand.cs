using MediatR;
using PoGoSearchGenerator.Domain.Entities;
using PoGoSearchGenerator.infrastructure.Efcore;
using PoGoSearchGenerator.infrastructure.PokeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PoGoSearchGenerator.Application.Commands.Type
{
    public class GetTypesCommand : IRequest<List<string>>
    {
    }

    public class GetTypesCommandHandler
        : IRequestHandler<GetTypesCommand, List<string>>
    {
        /// <summary>
        /// <see cref="ApplicationDbContext"/> to use
        /// </summary>
        private readonly ApplicationDbContext _context;

        public GetTypesCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<string>> Handle(GetTypesCommand request, CancellationToken cancellationToken)
        {
            //check if there are any types in the database 
            if (!_context.Set<Types>().Any())
            {
                //if not we call api for a list
                if (!await new PokeApiTypeGather(_context).GatherTypeListAsync())
                {
                    return null;
                }
            }

            //return a list of all types names
            return _context.Set<Types>().Select(x => x.Name).ToList();
        }
    }
}
