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

        public World(int width, int height)
        {
            this.height = height;
            this.width = width;
            Rooms = new Room[width, height];
            RoomsArray = new Room[width * height];
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
            return neighbors;
        }

        public Room GetRandomNeighbor(int roomID)
        {
            Neighbors neighbors = GetNeighbors(roomID);
            List<Room> rooms = neighbors.Rooms;


            int index = Random.Range(0, neighbors.Count);
            return rooms[index];
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