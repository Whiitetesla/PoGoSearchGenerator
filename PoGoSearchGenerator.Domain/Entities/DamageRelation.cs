using System;
using System.Collections.Generic;
using System.Text;

namespace PoGoSearchGenerator.Domain.Entities
{
    public class DamageRelation
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Types> Double_damage_from { get; set; } = new List<Types>();

        /// <summary>
        /// 
        /// </summary>
        public List<Types> Double_damage_to { get; set; } = new List<Types>();

        /// <summary>
        /// 
        /// </summary>
        public List<Types> Half_damage_from { get; set; } = new List<Types>();

        /// <summary>
        /// 
        /// </summary>
        public List<Types> No_damage_from { get; set; } = new List<Types>();
    }
}
