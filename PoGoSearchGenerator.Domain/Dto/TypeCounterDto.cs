using System;
using System.Collections.Generic;
using System.Text;

namespace PoGoSearchGenerator.Domain.Dto
{
    public class TypeCounterDto
    {
        /// <summary>
        /// what type you are against
        /// </summary>
        public List<string> Types { get; set; }

        /// <summary>
        /// what cp is the minimum
        /// </summary>
        public int CombatPoints { get; set; }

        /// <summary>
        /// what hp is the minimum
        /// </summary>
        public int HitPoints { get; set; }

        /// <summary>
        /// should we add STAB types
        /// </summary>
        public bool Stab { get; set; }

        /// <summary>
        /// should we add types that are weak to the type you are against
        /// </summary>
        public bool WeaknessType { get; set; }

        /// <summary>
        /// the relation between fast and charged
        /// if it is an & (and) or ,(or)
        /// </summary>
        public bool  AttackRelation { get; set; }
    }
}
