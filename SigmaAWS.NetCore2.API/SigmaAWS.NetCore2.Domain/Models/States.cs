using System.Collections.Generic;

namespace SigmaAWS.NetCore2.Domain.Models
{
    public partial class States
    {
        public States()
        {
            Customers = new HashSet<Customers>();
        }

        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }

        public ICollection<Customers> Customers { get; set; }
    }
}
