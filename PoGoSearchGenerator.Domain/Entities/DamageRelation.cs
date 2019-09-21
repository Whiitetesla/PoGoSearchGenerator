using System;
using System.Collections.Generic;
using System.Text;

namespace PoGoSearchGenerator.Domain.Entities
{
    public class DamageRelation
    {
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Double_damage_from { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> Double_damage_to { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> Half_damage_from { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> No_damage_from { get; set; } = new List<string>();
    }
}
