using System;

namespace PoGoSearchGenerator.infrastructure.PokeApi.Dto
{
    public class PokeApiTypeResultDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return Name == ((PokeApiTypeResultDto)obj).Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
