using System.Collections.Generic;

namespace SigmaAWS.NetCore2.Domain.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string LastName { get; set; }
        public int OrderCount { get; set; }
        public int StateId { get; set; }
        public int Zip { get; set; }

        public States State { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
