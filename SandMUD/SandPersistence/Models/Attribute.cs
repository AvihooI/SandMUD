namespace SandPersistence.Models
{
    public interface IAttribute
    {
        int AttributeId { get; set; }
        string AttributeName { get; set; }
        string AttributeValue { get; set; }
    }

    public class PlayerCharacterAttribute : IAttribute
    {
        public int PlayerCharacterId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

    public class NpcAttribute : IAttribute
    {
        public int NpcId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

    public class ItemAttribute : IAttribute
    {
        public int ItemId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

    public class RoomAttribute : IAttribute
    {
        public int RoomId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}