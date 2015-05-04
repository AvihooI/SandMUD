namespace SandModel
{
    public class InventoryItem //This defines a concrete item used by someone or placed somewhere
    {
        public int Id { get; set; }
        public TemplateItem Item { get; set; }
    }
}