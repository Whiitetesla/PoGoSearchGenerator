namespace PoGoSearchGenerator.Domain.Entities
{
    public class TypeDamageRelation
    {
        public int Id { get; set; }

        public int TypesId { get; set; }

        public Types Types { get; set; }

        public DamageRelation DoubleFrom { get; set; }

        public DamageRelation DoubleTo { get; set; }

        public DamageRelation HalfFrom { get; set; }

        public DamageRelation NoFrom { get; set; }
    }
}
