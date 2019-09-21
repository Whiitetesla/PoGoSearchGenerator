using System;
using System.Collections.Generic;
using System.Text;

namespace PoGoSearchGenerator.infrastructure.PokeApi.Dto
{
    public class PokeApiTypeDamageRelationDto
    {
        public Relations Damage_relations { get; set; }
        public string Name { get; set; }
    }

    public class Relations
    {
        /// <summary>
        /// 
        /// </summary>
        public PokeApiTypeResultDto[] Double_damage_from { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PokeApiTypeResultDto[] Double_damage_to { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PokeApiTypeResultDto[] Half_damage_from { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PokeApiTypeResultDto[] No_damage_from { get; set; }
    }
}
