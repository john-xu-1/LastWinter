using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class World
    {
        public string name = "World Name";
        public Graph worldGraph;
        public Dictionary<string, List<List<string>>> rawGraph;
        
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Room[] RoomsArray;
        private Room[,] Rooms;
        public Vector2Int startPos;

        public enum WorldStates
        {
            Building,
            Built,
            Completed
        }
        public WorldStates WorldState;

        public WorldHistory WorldHistory;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public World(int width, int height)
        {
            this.height = height;
            this.width = width;
            Rooms = new Room[width, height];
            RoomsArray = new Room[width * height];
            WorldHistory = new WorldHistory();
        }

        public void WorldHistoryAdd(int roomID, Map map, List<FreeObject> items, double buildTime, Clingo.ClingoSolver.Status status)
        {
            WorldHistory.AddRoom(roomID, map, items, buildTime, status);
        }
        public void WorldHistoryRemove(int roomID, Map map, List<FreeObject> items, double destroyTime, int destroyedByID, Clingo.ClingoSolver.Status status)
        {
            WorldHistory.DestroyRoom(roomID, map, items, destroyTime, destroyedByID, status);
        }
        public void WorldHistoryRemove(Room room, double destroyTime, int destroyedByID, Clingo.ClingoSolver.Status status)
        {
            int roomID = Utility.index_to_roomID(room.pos, width, height);
            WorldHistory.DestroyRoom(roomID, room.map, room.items, destroyTime, destroyedByID, status);
        }

        public Room[] GetRooms()
        {
            return RoomsArray;
        }

        public Room GetRoom(int x, int y)
        {
            if(Rooms[x,y] != null)
                return Rooms[x, y];
            else
            {
                Room room = new Room(new Vector2Int(x, y));
                Rooms[x, y] = room;
                RoomsArray[Utility.index_to_roomID(new Vector2Int(x, y), width, height)-1] = room;
                return room;
            }
        }
        public Room GetRoom(int roomID)
        {
            Vector2Int index = Utility.roomID_to_index(roomID, width, height);
            return GetRoom(index.x, index.y);
        }
        public List<int> GetNeighborIDs(int roomID)
        {
            Neighbors neighbors = GetNeighbors(roomID);
            List<int> indices = new List<int>();
            if (neighbors.left != null && !neighbors.left.isDestroyed) indices.Add(Utility.index_to_roomID(neighbors.left.pos, width, height));
            if (neighbors.right != null && !neighbors.right.isDestroyed) indices.Add(Utility.index_to_roomID(neighbors.right.pos, width, height));
            if (neighbors.up != null && !neighbors.up.isDestroyed) indices.Add(Utility.index_to_roomID(neighbors.up.pos, width, height));
            if (neighbors.down != null && !neighbors.down.isDestroyed) indices.Add(Utility.index_to_roomID(neighbors.down.pos, width, height));
            return indices;
        }
        public Neighbors GetNeighbors(int roomID)
        {
            Vector2Int index = Utility.roomID_to_index(roomID, width, height);
            return GetNeighbors(index);
        }

        public Neighbors GetNeighbors(Vector2Int index)
        {
            return GetNeighbors(index.x, index.y);
        }
        public Neighbors GetNeighbors(Room room)
        {
            return GetNeighbors(room.pos);
        }

        public Neighbors GetNeighbors(int x, int y)
        {

            Neighbors neighbors = new Neighbors();
            if (x > 0 && Rooms[x - 1, y] != null && !Rooms[x - 1, y].isDestroyed) neighbors.left = Rooms[x - 1, y];
            if (x < width - 1 && Rooms[x + 1, y] != null && !Rooms[x + 1, y].isDestroyed) neighbors.right = Rooms[x + 1, y];
            if (y > 0 && Rooms[x, y - 1] != null && !Rooms[x, y - 1].isDestroyed) neighbors.up = Rooms[x, y - 1];
            if (y < height - 1 && Rooms[x, y + 1] != null && !Rooms[x, y + 1].isDestroyed) neighbors.down = Rooms[x, y + 1];

            Room room = GetRoom(x, y);
            if(room != null && room.buidStatus == Clingo.ClingoSolver.Status.UNSATISFIABLE)
            {
                List<int> removed = room.removedNeighbors;
                if (neighbors.left != null && removed.Contains(Utility.index_to_roomID(neighbors.left.pos, width, height))) neighbors.left = null;
                if (neighbors.right != null && removed.Contains(Utility.index_to_roomID(neighbors.right.pos, width, height))) neighbors.right = null;
                if (neighbors.up != null && removed.Contains(Utility.index_to_roomID(neighbors.up.pos, width, height))) neighbors.up = null;
                if (neighbors.down != null && removed.Contains(Utility.index_to_roomID(neighbors.down.pos, width, height))) neighbors.down = null;
            }

            return neighbors;
        }

        public Neighbors GetNeighborsWithOpening(int roomID)
        {
            Neighbors neighbors = GetNeighbors(roomID);
            if (neighbors.left != null && !Utility.hasOpening(neighbors.left.map, "right")) neighbors.left = null;
            if (neighbors.right != null && !Utility.hasOpening(neighbors.right.map, "left")) neighbors.right = null;
            if (neighbors.up != null && !Utility.hasOpening(neighbors.up.map, "down")) neighbors.up = null;
            if (neighbors.down != null && !Utility.hasOpening(neighbors.down.map, "up")) neighbors.down = null;

            return neighbors;
        }

        public Room GetRandomNeighbor(int roomID)
        {
            Neighbors neighbors = GetNeighborsWithOpening(roomID);
            if(neighbors.Count <= 0)
            {
                return null;
            }
            else
            {
                List<Room> rooms = neighbors.Rooms;
                int index = Random.Range(0, neighbors.Count);
                return rooms[index];
            }
            
        }
        public int GetStartRoom()
        {
            return worldGraph.startRoomID;
        }
        public Vector2Int GetStartPos()
        {
            int startRoomID = GetStartRoom();
            Room startRoom = GetRoom(startRoomID);
            int x = 0;
            int y = 0;
            foreach(PathStart pathStart in startRoom.map.pathStarts)
            {
                if(pathStart.type == "middle")
                {
                    x = pathStart.x;
                    y = pathStart.y;
                    break;
                }
            }
            int width = startRoom.map.dimensions.room_width;
            int height = startRoom.map.dimensions.room_height;
            return new Vector2Int(width * startRoom.pos.x + x, height * startRoom.pos.y + y);
        }

        public void FinishWorldBuild()
        {
            WorldState = World.WorldStates.Built;
            startPos = GetStartPos();

        }
    }

    public class Neighbors
    {
        public Room left, right, up, down;
        public List<Room> Rooms
        {
            get
            {
                List<Room> rooms = new List<Room>();
                if (up != null && !up.isDestroyed) rooms.Add(up);
                if (right != null && !right.isDestroyed) rooms.Add(right);
                if (down != null && !down.isDestroyed) rooms.Add(down);
                if (left != null && !left.isDestroyed) rooms.Add(left);
                return rooms;
            }
        }
        public override string ToString()
        {
            string display = "";
            if (up != null) display += " up: " + up.pos + " ";
            if (right != null) display += " right: " + right.pos + " ";
            if (down != null) display += " down: " + down.pos + " ";
            if (left != null) display += " left: " + left.pos;
            return display;
        }
        public int Count {
            get
            {
                int count = 0;
                if (up != null && !up.isDestroyed) count += 1;
                if (right != null && !right.isDestroyed) count += 1;
                if (down != null && !down.isDestroyed) count += 1;
                if (left != null && !left.isDestroyed) count += 1;
                return count;
            }
        }

        //public Room RandomNeighbor()
        //{
        //    if(Count > 0)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}