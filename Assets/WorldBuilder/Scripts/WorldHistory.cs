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
        public RoomHistory[] history;
        public void AddRoom(int roomID, Map map, List<FreeObject> items, double buildTime, Clingo_00.ClingoSolver.Status status)
        {
            roomHistories.Add(new RoomHistory(roomID, map, items, buildTime, status));
        }
        public void DestroyRoom(int roomID, Map map, List<FreeObject> items, double destroyTime, int destroyedByID, Clingo_00.ClingoSolver.Status status)
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
            roomHistories.Add(new RoomHistory(roomID, map, items, destroyTime, destroyedByID, status));
        }
        public RoomHistory GetRoom(int index)
        {
            if (history.Length > 0) return history[index];
            else return roomHistories[index];
        }

        public double GetTotalTime()
        {
            double totalTime = 0;
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
                double totalTime = 0;
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
        public List<FreeObject> items;
        public double buildTime;
        public bool destroyed;
        public int destroyedByID;
        public Clingo_00.ClingoSolver.Status buildStatus;

        public RoomHistory(int roomID, Map map, List<FreeObject> items, double buildTime, Clingo_00.ClingoSolver.Status status)
        {
            this.roomID = roomID;
            this.map = map;
            this.buildTime = buildTime;
            this.items = items;
            roomName = "Room " + roomID.ToString() + " BuildTime: " + buildTime.ToString();
            buildStatus = status;
        }
        public RoomHistory(int roomID, Map map, List<FreeObject> items, double destroyTime, int destroyedByID, Clingo_00.ClingoSolver.Status status)
        {
            buildTime = destroyTime;
            this.roomID = roomID;
            this.map = map;
            this.items = items;
            buildStatus = status;
            DestroyRoom(destroyedByID);
            roomName = "Destroyed Room " + roomID.ToString() + " " + status;
            
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
        public double averageBuildTime;
        public int buildCount;
    }
}