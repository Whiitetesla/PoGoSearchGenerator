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

            //create list of all types that is related to our types
            var damageScores = new List<DamageRelationScore>();

            //create the scores we need to add to the types
            var double_damage_from_score = 1.6;
            var half_damage_from_score = 0.6;
            var no_damage_from_score = 0;

            //loops the damageRelations to get all the types and add there scores to them
            foreach (var damageRelation in damageRelations)
            {
                foreach (var obj in damageRelation.Double_damage_from)
                {
                    var type = _context.Set<Types>().Find(obj.TypesId);
                    if (type == null)
                        continue;

                    if (damageScores.Any(x => x.Name == type.Name))
                        damageScores.FirstOrDefault(x => x.Name == type.Name).Score *= double_damage_from_score;

                    else
                        damageScores.Add(new DamageRelationScore()
                        {
                            Name = type.Name,
                            Score = double_damage_from_score
                        });
                }

                foreach (var obj in damageRelation.Half_damage_from)
                {
                    var type = _context.Set<Types>().Find(obj.TypesId);
                    if (type == null)
                        continue;

                    if (damageScores.Any(x => x.Name == type.Name))
                        damageScores.FirstOrDefault(x => x.Name == type.Name).Score *= half_damage_from_score;

                    else
                        damageScores.Add(new DamageRelationScore()
                        {
                            Name = type.Name,
                            Score = half_damage_from_score
                        });
                }

                foreach (var obj in damageRelation.No_damage_from)
                {
                    var type = _context.Set<Types>().Find(obj.TypesId);
                    if (type == null)
                        continue;

                    if (damageScores.Any(x => x.Name == type.Name))
                        damageScores.FirstOrDefault(x => x.Name == type.Name).Score *= no_damage_from_score;

                    else
                        damageScores.Add(new DamageRelationScore()
                        {
                            Name = type.Name,
                            Score = no_damage_from_score
                        });
                }

            }

            //create string to hold the information we need to return
            var returnStr = "";

            //the minimum score has to be to count as "super effective"
            var powerMin = 1.5;

            //change the minimum score to only coun't double "super effectives"
            if (request.TypeConter.DoubleSuper)
                powerMin = 2.5;

            //get the first type from the list that has a score more than our powerMin
            var firstSuper = damageScores.FirstOrDefault(x => x.Score > powerMin);

            //check to see if we have any "super effective" moves to work with 
            //and only weakness isn't true
            if (firstSuper != null && !request.TypeConter.OnlyWeakness)
            {
                //we there is requested for STAP we add the types
                if (request.TypeConter.Stab)
                {
                    returnStr += firstSuper.Name;

                    foreach (var type in damageScores.Where(x => x.Score > powerMin && x.Name != firstSuper.Name))
                    {
                        returnStr += $",{type.Name}";
                    }

                    returnStr += "&";
                }

                //setup for all fast attacks
                returnStr += $"@1{firstSuper.Name}";

                foreach (var type in damageScores.Where(x => x.Score > powerMin && x.Name != firstSuper.Name))
                {
                    returnStr += $",@1{type.Name}";
                }

                //use the AttackRelation to determan if we need to use & or ,
                if (request.TypeConter.AttackRelation)
                    returnStr += "&";
                else
                    returnStr += ",";

                //setup for charged and extra moves
                returnStr += $"@2{firstSuper.Name},@3{firstSuper.Name}";

                foreach (var type in damageScores.Where(x => x.Score > powerMin && x.Name != firstSuper.Name))
                {
                    returnStr += $",@2{type.Name},@3{type.Name}";
                }

                returnStr += "&";
            }

            //add cp and hp limit
            returnStr += $"cp{request.TypeConter.CombatPoints}-";
            returnStr += $"&hp{request.TypeConter.HitPoints}-";

            //check to see if we need to add a removal for types that is weak to the type attack
            if (request.TypeConter.WeaknessType)
            {
                var typeNames = new List<string>();

                //loop damageRelation to get all types its strong against
                foreach (var damageRelation in damageRelations)
                {
                    foreach (var typeRelation in damageRelation.Double_damage_to)
                    {
                        var type = _context.Set<Types>().Find(typeRelation.TypesId);
                        if (type == null)
                            continue;

                        if (!typeNames.Any(x => x == type.Name))
                            typeNames.Add(type.Name);
                    }
                }

                //adds all the types that is weak to the list
                foreach (var typeName in typeNames)
                {
                    //adds not operation to the list for removal of the types
                    returnStr += $"&!{typeName}";
                }
            }

            if (request.TypeConter.WeaknessMove)
            {
                var firstWeak = damageScores.FirstOrDefault(x => x.Score < 0.7);

                if(firstWeak != null)
                {
                    //setup for all fast attacks
                    returnStr += $"&!@1{firstWeak.Name}&!@2{firstWeak.Name}&!@3{firstWeak.Name}";

                    foreach (var type in damageScores.Where(x => x.Score < 0.7 && x.Name != firstWeak.Name))
                    {
                        returnStr += $"&!@1{type.Name}&!@2{type.Name}&!@3{type.Name}";
                    }
                }
            }

            return returnStr;
        }
        
    }

    public class DamageRelationScore
    {
        public string Name { get; set; }
        public double Score { get; set; }
    }
}
