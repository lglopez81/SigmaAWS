namespace SigmaAWS.NetCore2.Domain.Models
{
    public partial class Orders
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public decimal Price { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }

        public Customers Customers { get; set; }
    }
}
