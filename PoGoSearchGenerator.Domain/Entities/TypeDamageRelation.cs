using System;
using System.Collections.Generic;
using System.Text;

namespace PoGoSearchGenerator.Domain.Entities
{
    public class TypeDamageRelation
    {
        public int Id { get; set; }

        public int TypesId { get; set; }

        public Types Types { get; set; }

        public int DamageRelationId { get; set; }

        public DamageRelation DamageRelation { get; set; }
    }
}
