using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        [InverseProperty("DoubleFrom")]
        public List<TypeDamageRelation> Double_damage_from { get; set; } = new List<TypeDamageRelation>();

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty("DoubleTo")]
        public List<TypeDamageRelation> Double_damage_to { get; set; } = new List<TypeDamageRelation>();

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty("HalfFrom")]
        public List<TypeDamageRelation> Half_damage_from { get; set; } = new List<TypeDamageRelation>();

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty("NoFrom")]
        public List<TypeDamageRelation> No_damage_from { get; set; } = new List<TypeDamageRelation>();
    }
}
