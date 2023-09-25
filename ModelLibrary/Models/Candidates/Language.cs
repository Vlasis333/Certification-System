using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Candidates
{
    public class Language
    {
        public int Id { get; set; }
        public string? NativeLanguage { get; set; }

        public virtual ICollection<Candidate>?
            Candidates { get; set; } // NOTE:(akotro) Reverse navigation
    }
}
