
using System.Collections.Generic;

namespace SandPersistence
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public string RoomDescription { get; set; }
        public List<RoomExit> Exits { get; set; }
    }

    public class RoomExit
    {
        public string Direction;
        public int RoomID;

        public int EntranceRoomID;
    }
}