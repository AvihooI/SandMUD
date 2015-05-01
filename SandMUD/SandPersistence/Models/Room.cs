using System.Collections.Generic;

namespace SandPersistence.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string RoomDescription { get; set; }
        public List<RoomExit> Exits { get; set; }
    }

    public class RoomExit
    {
        public string Direction;
        public int EntranceRoomId;
        public int RoomId;
    }
}