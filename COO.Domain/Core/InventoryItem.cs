using System.Collections.Generic;

namespace COO.Domain.Core
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int SlotIndex { get; set; }
        public bool Rotated { get; set; }
        public int Capacity { get; set; }
        public List<ContainerItem> ContainerItems { get; set; }
        //public string Item { get; set; }
        //public int Slot { get; set; }
        //public int Amount { get; set; }
    }
}
