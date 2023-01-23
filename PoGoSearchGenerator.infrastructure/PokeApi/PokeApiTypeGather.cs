using Newtonsoft.Json;
using PoGoSearchGenerator.Domain.Entities;
using PoGoSearchGenerator.infrastructure.Efcore;
using PoGoSearchGenerator.infrastructure.PokeApi.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PoGoSearchGenerator.infrastructure.PokeApi
{
    public class PokeApiTypeGather
    {
        private readonly ApplicationDbContext _context;

        public PokeApiTypeGather(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async System.Threading.Tasks.Task<bool> GatherTypeListAsync()
        {
            var TempTypes = new List<Types>();

            //call api for list of all types
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://pokeapi.co/api/v2/type");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                //create a model from the dto
                var jsonString = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<PokeApiTypeListDto>(jsonString);

                //loop the list to convert it to Types class
                foreach (var obj in model.Results)
                {
                    //ingores unkown and shadown types
                    //sinces they don't apear in GO
                    if (obj.Name != "unknown" && obj.Name != "shadow")
                    {
                        TempTypes.Add(new Types()
                        {
                            Name = obj.Name
                        });
                    }
                }

                //saves types
                _context.Set<Types>().AddRange(TempTypes);
                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}
