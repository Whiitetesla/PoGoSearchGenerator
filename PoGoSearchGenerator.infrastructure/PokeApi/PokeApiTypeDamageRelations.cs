using Newtonsoft.Json;
using PoGoSearchGenerator.Domain.Entities;
using PoGoSearchGenerator.infrastructure.Efcore;
using PoGoSearchGenerator.infrastructure.PokeApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace PoGoSearchGenerator.infrastructure.PokeApi
{
    public class PokeApiTypeDamageRelations
    {
        private readonly ApplicationDbContext _context;

        public PokeApiTypeDamageRelations(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async System.Threading.Tasks.Task GatherGetDamageRelation(string type)
        {
            //check if we have the type we search for in the db
            if (!_context.Set<Types>().Any(x => x.Name == type))
                await new PokeApiTypeGather(_context).GatherTypeListAsync();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://pokeapi.co/api/v2/type/{type}");
            if (response.IsSuccessStatusCode)
            {
                //create a model from the dto
                var jsonString = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<PokeApiTypeDamageRelationDto>(jsonString);

                //create new object to store the information to add to the list
                DamageRelation damageRelation = new DamageRelation
                {
                    Type = model.Name
                };

                //moves all the lists to our new damageRelation
                damageRelation.Double_damage_from = 
                        _context.Set<Types>()
                        .Where(x =>
                            model.Damage_relations.Double_damage_from
                            .Contains(new PokeApiTypeResultDto()
                            {
                                Name = x.Name
                            }))
                        .ToList();

                damageRelation.Double_damage_to =
                        _context.Set<Types>()
                        .Where(x =>
                            model.Damage_relations.Double_damage_to
                            .Contains(new PokeApiTypeResultDto()
                            {
                                Name = x.Name
                            }))
                        .ToList();

                damageRelation.Half_damage_from =
                        _context.Set<Types>()
                        .Where(x =>
                            model.Damage_relations.Half_damage_from
                            .Contains(new PokeApiTypeResultDto()
                            {
                                Name = x.Name
                            }))
                        .ToList();

                damageRelation.No_damage_from =
                        _context.Set<Types>()
                        .Where(x =>
                            model.Damage_relations.No_damage_from
                            .Contains(new PokeApiTypeResultDto()
                            {
                                Name = x.Name
                            }))
                        .ToList();

                //save data to db
                _context.Set<DamageRelation>().Add(damageRelation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
