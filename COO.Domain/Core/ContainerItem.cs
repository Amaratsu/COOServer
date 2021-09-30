namespace COO.Domain.Core
{
    public class ContainerItem
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int SlotIndex { get; set; }
        public int ContainerIndex { get; set; }
        public bool Rotated { get; set; }
    }
}
