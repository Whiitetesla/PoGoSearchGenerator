using Newtonsoft.Json;
using PoGoSearchGenerator.Domain.Entities;
using PoGoSearchGenerator.infrastructure.Efcore;
using PoGoSearchGenerator.infrastructure.PokeApi.Dto;
using System;
using System.Linq;
using System.Net.Http;

namespace PoGoSearchGenerator.infrastructure.PokeApi
{
    public class PokeApiTypeDamageRelations
    {
        private readonly ApplicationDbContext _context;

        public PokeApiTypeDamageRelations(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async System.Threading.Tasks.Task<bool> GatherGetDamageRelation(string type)
        {
            //check if we have the type we search for in the db
            if (!_context.Set<Types>().Any(x => x.Name == type))
            {
                if (!await new PokeApiTypeGather(_context).GatherTypeListAsync())
                {
                    return false;
                }
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://pokeapi.co/api/v2/type/{type}");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                //create a model from the dto
                var jsonString = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<PokeApiTypeDamageRelationDto>(jsonString);

                //create new object to store the information to add to the list
                var damageRelation = new DamageRelation
                {
                    Type = model.Name
                };

                //moves all the lists to our new damageRelation
                damageRelation.Double_damage_from
                    = model.Damage_relations.Double_damage_from
                    .Select(x => new TypeDamageRelation()
                    {
                        TypesId = _context.Set<Types>().FirstOrDefault(y => y.Name == x.Name).Id
                    }).ToList();

                damageRelation.Double_damage_to
                    = model.Damage_relations.Double_damage_to
                    .Select(x => new TypeDamageRelation()
                    {
                        TypesId = _context.Set<Types>().FirstOrDefault(y => y.Name == x.Name).Id
                    }).ToList();

                damageRelation.Half_damage_from
                    = model.Damage_relations.Half_damage_from
                    .Select(x => new TypeDamageRelation()
                    {
                        TypesId = _context.Set<Types>().FirstOrDefault(y => y.Name == x.Name).Id
                    }).ToList();

                damageRelation.No_damage_from
                    = model.Damage_relations.No_damage_from
                    .Select(x => new TypeDamageRelation()
                    {
                        TypesId = _context.Set<Types>().FirstOrDefault(y => y.Name == x.Name).Id
                    }).ToList();

                //save data to db
                _context.Set<DamageRelation>().Add(damageRelation);
                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}
