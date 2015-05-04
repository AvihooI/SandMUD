using System.Collections.Generic;

namespace SandModel
{
    public interface IHasInventory
    {
        ICollection<InventoryItem> Inventory { get; set; }
    }
}