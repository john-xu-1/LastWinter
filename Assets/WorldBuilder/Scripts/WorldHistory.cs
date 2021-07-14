using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class WorldHistory 
    {

        [SerializeField] private List<RoomHistory> roomHistories = new List<RoomHistory>();
        [SerializeField] private RoomHistoryAnalysis[] roomHistoryAnalysis;
        public void AddRoom(int roomID, Map map, float buildTime)
        {
            roomHistories.Add(new RoomHistory(roomID, map, buildTime));
        }
        public void DestroyRoom(int roomID, Map map, float destroyTime, int destroyedByID)
        {
            Map destroyedMap = new Map();
            destroyedMap.dimensions =  map.dimensions;
            destroyedMap.area = new Tile[map.area.Length];
            //Debug.Log("DestroyMap length: " + map.area.Length);
            for(int i = 0; i < destroyedMap.area.Length; i += 1)
            {
                
                Tile tile = map.area[i];
                destroyedMap.area[i] = new Tile();
                //Debug.Log("area index: " + destroyedMap.area[i].x);
                destroyedMap.area[i].x = tile.x;
                destroyedMap.area[i].y = tile.y;
                destroyedMap.area[i].type = 0;

            }
            roomHistories.Add(new RoomHistory(roomID, map, destroyTime, destroyedByID));
        }
        public RoomHistory GetRoom(int index)
        {
            return roomHistories[index];
        }

        public float GetTotalTime()
        {
            float totalTime = 0;
            foreach(RoomHistory roomHistory in roomHistories)
            {
                totalTime += roomHistory.buildTime;
            }
            return totalTime;
        }
        public void GetRoomHistoryAnalysis()
        {
            int roomCount = 0;
            foreach(RoomHistory roomHistory in roomHistories)
            {
                if (roomHistory.roomID > roomCount) roomCount = roomHistory.roomID;
            }

            roomHistoryAnalysis = new RoomHistoryAnalysis[roomCount];
            for (int roomID = 1; roomID <= roomCount; roomID += 1)
            {
                float totalTime = 0;
                int buildCount = 0;
                foreach (RoomHistory roomHistory in roomHistories)
                {
                    if (roomHistory.roomID == roomID && !roomHistory.destroyed)
                    {
                        totalTime += roomHistory.buildTime;
                        buildCount += 1;
                    }
                }

                RoomHistoryAnalysis newAnalysis = new RoomHistoryAnalysis();
                newAnalysis.roomID = roomID.ToString();
                newAnalysis.buildCount = buildCount;
                newAnalysis.averageBuildTime = totalTime / buildCount;
                roomHistoryAnalysis[roomID - 1] = newAnalysis;
            }
        }
    }

    [System.Serializable]
    public class RoomHistory
    {
        public string roomName;
        public int roomID;
        public Map map;
        public float buildTime;
        public bool destroyed;
        public int destroyedByID;

        public RoomHistory(int roomID, Map map, float buildTime)
        {
            this.roomID = roomID;
            this.map = map;
            this.buildTime = buildTime;
            roomName = "Room " + roomID.ToString() + " BuildTime: " + buildTime.ToString();
        }
        public RoomHistory(int roomID, Map map, float destroyTime, int destroyedByID)
        {
            buildTime = destroyTime;
            this.roomID = roomID;
            this.map = map;
            DestroyRoom(destroyedByID);
            roomName = "Destroyed Room " + roomID.ToString();
        }

        public void DestroyRoom(int destroyedByID)
        {
            destroyed = true;
            this.destroyedByID = destroyedByID;
        }

        
    }

    [System.Serializable]
    public class RoomHistoryAnalysis
    {
        public string roomID;
        public float averageBuildTime;
        public int buildCount;
    }
}