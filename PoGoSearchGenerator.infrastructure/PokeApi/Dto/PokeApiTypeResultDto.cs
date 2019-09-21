using System;
using System.Collections.Generic;
using System.Text;

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
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;

            return this.Name == ((PokeApiTypeResultDto)obj).Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
