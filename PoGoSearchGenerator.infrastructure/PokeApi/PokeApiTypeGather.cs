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
    public class PokeApiTypeGather
    {
        private readonly ApplicationDbContext _context;

        public PokeApiTypeGather(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //if the list is empty we gahter it form the api
        public async System.Threading.Tasks.Task GatherTypeListAsync()
        {
            List<Types> TempTypes = new List<Types>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://pokeapi.co/api/v2/type");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<PokeApiTypeListDto>(jsonString);
                foreach (var obj in model.Results)
                {
                    if (obj.Name != "unknown" && obj.Name != "shadow")
                        TempTypes.Add(new Types()
                        {
                           Name = obj.Name
                        });
                }

                _context.Set<Types>().AddRange(TempTypes);

                await _context.SaveChangesAsync();
            }
        }
    }
}
