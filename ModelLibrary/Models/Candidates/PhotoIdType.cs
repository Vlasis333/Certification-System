using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Candidates
{
    public enum PhotoIdTypeEnum
    {
        Passport,
        National,
        Millitary,
        DriversLicense
    }

    public class PhotoIdType
    {
        public int Id { get; set; }
        public PhotoIdTypeEnum? IdType { get; set; }

        public virtual ICollection<Candidate>? Candidates { get; set; } // NOTE:(akotro) Reverse navigation
    }
}
