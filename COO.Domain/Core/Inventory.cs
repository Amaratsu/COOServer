namespace COO.Domain.Core
{
    public class Inventory
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string Item { get; set; }
        public int Slot { get; set; }
        public int Amount { get; set; }
    }
}
