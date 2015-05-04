using System.Collections.Generic;

namespace Sand.Model
{
    public interface IHasInventory
    {
        ICollection<InventoryItem> Inventory { get; set; }
    }
}