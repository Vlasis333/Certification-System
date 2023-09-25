using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Candidates
{
    public enum GenderEnum
    {
        Male,
        Female,
        Other
    }

    public class Gender
    {
        public int Id { get; set; }
        public GenderEnum? GenderType { get; set; }

        public virtual ICollection<Candidate>? Candidates { get; set; } // NOTE:(akotro) Reverse navigation
    }
}
