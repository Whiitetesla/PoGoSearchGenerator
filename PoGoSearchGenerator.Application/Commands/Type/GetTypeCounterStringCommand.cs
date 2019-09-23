using MediatR;
using Microsoft.EntityFrameworkCore;
using PoGoSearchGenerator.Domain.Dto;
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
    public class GetTypeCounterStringCommand : IRequest<string>
    {
        public TypeCounterDto TypeConter{ get; set; }

        public GetTypeCounterStringCommand(TypeCounterDto typeConter)
        {
            TypeConter = typeConter ?? throw new ArgumentNullException(nameof(typeConter));
        }
    }

    public class GetTypeCounterStringCommandHandler
        : IRequestHandler<GetTypeCounterStringCommand, string>
    {
        /// <summary>
        /// <see cref="ApplicationDbContext"/> to use
        /// </summary>
        private readonly ApplicationDbContext _context;

        public GetTypeCounterStringCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<string> Handle(GetTypeCounterStringCommand request, CancellationToken cancellationToken)
        {
            foreach (var type in request.TypeConter.Types)
            {
                //check if we have a damageRelation with the type we search for
                if (!_context.Set<DamageRelation>().Any(x => x.Type == type))
                    //if not we all api for the information
                    if (!await new PokeApiTypeDamageRelations(_context).GatherGetDamageRelation(type))
                        return null;
            }

            //gahter all damageRelation we need from the db
            var damageRelations = 
                _context.Set<DamageRelation>()
                .Include(x => x.Double_damage_from)
                .Include(x => x.Double_damage_to)
                .Include(x => x.Half_damage_from)
                .Include(x => x.No_damage_from)
                .Where(x => request.TypeConter.Types.Contains(x.Type))
                .ToList();

            var damageScores = new List<DamageRelationScore>();
            var double_damage_from_score = 1.6;
            var half_damage_from_score = 0.6;
            var no_damage_from_score = 0.3;


            foreach (var damageRelation in damageRelations)
            {
                foreach (var obj in damageRelation.Double_damage_from)
                {
                    if (damageScores.Any(x => x.Name == obj.Name))
                        damageScores.FirstOrDefault(x => x.Name == obj.Name).Score *= double_damage_from_score;

                    else
                        damageScores.Add(new DamageRelationScore()
                        {
                            Name = obj.Name,
                            Score = double_damage_from_score
                        });
                }

                foreach (var obj in damageRelation.Half_damage_from)
                {
                    if (damageScores.Any(x => x.Name == obj.Name))
                        damageScores.FirstOrDefault(x => x.Name == obj.Name).Score *= half_damage_from_score;

                    else
                        damageScores.Add(new DamageRelationScore()
                        {
                            Name = obj.Name,
                            Score = half_damage_from_score
                        });
                }

                foreach (var obj in damageRelation.No_damage_from)
                {
                    if (damageScores.Any(x => x.Name == obj.Name))
                        damageScores.FirstOrDefault(x => x.Name == obj.Name).Score *= no_damage_from_score;

                    else
                        damageScores.Add(new DamageRelationScore()
                        {
                            Name = obj.Name,
                            Score = no_damage_from_score
                        });
                }

            }

            //create new clas for damage claculation
            //Double_damage_from = 1.6
            //Half_damage_from = 0.6
            //No_damage_from = 0.3
            //add of same type together in class prop
            //return all with damage number above 1.5

            return null;
        }
        
    }

    public class DamageRelationScore
    {
        public string Name { get; set; }
        public double Score { get; set; }
    }
}
