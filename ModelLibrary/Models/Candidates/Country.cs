using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Candidates
{
    public class Country
    {
        public int Id { get; set; }
        public string? CountryOfResidence { get; set; }

        public virtual ICollection<Address>? Addresses { get; set; } // NOTE:(akotro) Reverse navigation
    }
}
