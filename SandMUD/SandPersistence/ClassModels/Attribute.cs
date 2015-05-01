
namespace SandPersistence
{
    public interface IAttribute
    {
        int AttributeID { get; set; }
        string AttributeName { get; set; }
        string AttributeValue { get; set; }
    }

    public class PlayerCharacterAttribute : IAttribute
    {
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int PlayerCharacterID { get; set; }
    }

    public class NpcAttribute : IAttribute
    {
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int NpcID { get; set; }
    }

    public class ItemAttribute: IAttribute
    {
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int ItemID { get; set; }
    }

    public class RoomAttribute: IAttribute
    {
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int RoomID { get; set; }
    }
}