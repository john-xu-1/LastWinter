using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugging
{
    public class DebugData : MonoBehaviour
    {
        [SerializeField] string testDataPath = "GraphCSVTest1";
        string[,] runtimeData;

        public void RuntimeDataStart(int col)
        {
            //runtimeData = new string[col, rows];
            runtimeData = new string[col, 0];
        }
        public void RuntimeDataStart(int col, int rows)
        {
            runtimeData = new string[col, rows];
            
        }
        public void RuntimeDataBatchEnd()
        {
            string[] space = new string[runtimeData.GetUpperBound(0) + 1];
            runtimeData = DebugUtility.AddRow(runtimeData, space);
        }
        public void RuntimeData(float value, int runID, int col)
        {
            RuntimeData(value, runID, col, 0, 0, 0);
        }
        public void RuntimeData(float value, int runID, int cpu, int width, int height, int keyType)
        {

            runtimeData[cpu, runID] = value.ToString();
            Debug.Log($"x:{cpu},y:{runID} = {runtimeData[cpu, runID]}");
        }
        public void RuntimeData(float[] values, int runID)
        {
            for (int i = 0; i < values.Length; i += 1)
            {
                runtimeData[i, runID] = values[i].ToString();
            }
        }
        public void RuntimeData(string[] values, int runID)
        {
            runtimeData = DebugUtility.AddRow(runtimeData, values);


            Debug.Log(DebugUtility.ConvertMatrixToString(runtimeData));
            //if (runID >= runtimeData.GetUpperBound(1))
            //{

            //}
            //else
            //{
            //    for (int i = 0; i < values.Length; i += 1)
            //    {
            //        runtimeData[i, runID] = values[i];
            //    }
            //}

        }
        public void FinishRuntimeData()
        {
            string table = DebugUtility.CreateCSV(runtimeData);
            Debug.Log(table);
            DebugUtility.CreateFile(table, $"{testDataPath}.csv", DebugUtility.DataFilePath);
        }

        public void RoomRuntimeData(int runID, float runtime, DebugRoom debugRoom, WorldBuilder.Room room, Clingo.ClingoSolver.Status buildStatus)
        {
            string[] data = new string[22];
            Vector2Int roomIndex = room.pos;                                                                            
            int width = debugRoom.roomSize.x;                                                                           
            int height = debugRoom.roomSize.y;                                                                          
            int upExit = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE && debugRoom.connections.upEgress ? room.upExit.x : 0;                                            
            int upEntrace = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE && debugRoom.connections.upIngress ? room.upExit.x : 0;                                        
            int rightExit = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE && debugRoom.connections.rightEgress ? room.rightExit.y : 0;                                   
            int rightEntrance = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE && debugRoom.connections.rightIngress ? room.rightExit.y : 0;                              
            int downExit = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE && debugRoom.connections.downEgress ? room.downExit.x : 0;                                      
            int downEntrance = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE && debugRoom.connections.downIngress ? room.downExit.x : 0;                                 
            int leftExit = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE && debugRoom.connections.leftEgress ? room.leftExit.y : 0;                                      
            int leftEntrance = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE && debugRoom.connections.leftIngress ? room.leftExit.y : 0;                                 
            string gate = GetGateData(debugRoom)[0];                                                                    
            string gated = GetGateData(debugRoom)[1];                                                                   
            
            string neighborUp = GetNeighborBorder(debugRoom.neighbors, WorldBuilder.Utility.Directions.up);             
            string neighborRight = GetNeighborBorder(debugRoom.neighbors, WorldBuilder.Utility.Directions.right);                                                                                  
            string neighborDown = GetNeighborBorder(debugRoom.neighbors, WorldBuilder.Utility.Directions.down);                                                                                   
            string neighborLeft = GetNeighborBorder(debugRoom.neighbors, WorldBuilder.Utility.Directions.left);                                                                                   
            string status = buildStatus.ToString();                                                                     
            string map = buildStatus == Clingo.ClingoSolver.Status.SATISFIABLE? DebugUtility.GetASCIRoom(room): "";                                                                
            int cpus = debugRoom.cpus;                                                                                  
                                                                                                                        
            WorldBuilder.GateTypes key = debugRoom.key != null && debugRoom.key.type != 0 ?  debugRoom.gates[debugRoom.key.type - 1] : WorldBuilder.GateTypes.none;

            data[0] = $"{roomIndex.x} {roomIndex.y}";
            data[1] = width.ToString();
            data[2] = height.ToString();
            data[3] = upExit.ToString();
            data[4] = upEntrace.ToString();
            data[5] = rightExit.ToString();
            data[6] = rightEntrance.ToString();
            data[7] = downExit.ToString();
            data[8] = downEntrance.ToString();
            data[9] = leftExit.ToString();
            data[10] = leftEntrance.ToString();
            data[11] = key.ToString();
            data[12] = gate;
            data[13] = gated;
            data[14] = neighborUp;
            data[15] = neighborRight;
            data[16] = neighborDown;
            data[17] = neighborLeft;
            data[18] = status;
            data[19] = map;
            data[20] = cpus.ToString();
            data[21] = runtime.ToString();

            Debug.Log($"keyType = {key.ToString()}");
            RuntimeData(data, runID);
        }
        public void RoomRuntimeData(int runID, float runtime, DebugRoom debugRoom, Vector2Int roomIndex, Clingo.ClingoSolver.Status buildStatus)
        {
            WorldBuilder.Room room = new WorldBuilder.Room(roomIndex);
            room.isDestroyed = true;
            RoomRuntimeData(runID, runtime, debugRoom, room, buildStatus);
        }

        public string[] GetGateData(DebugRoom debugRoom)
        {
            string[] gateData = { "none", "none" };
            if (debugRoom.gate != null && debugRoom.gate.type != 0)
            {
                gateData[0] = debugRoom.gates[debugRoom.gate.type - 1].ToString();
                int source = debugRoom.gate.source;
                int destination = debugRoom.gate.destination;
                if (destination == source - 1) gateData[1] = "left";
                else if (destination == source + 1) gateData[1] = "right";
                else if (destination > source) gateData[1] = "down";
                else if (destination < source) gateData[1] = "up";
            }
            else if (debugRoom.gated != null && debugRoom.gated.type != 0)
            {
                gateData[0] = debugRoom.gates[debugRoom.gated.type - 1].ToString();
            }
            return gateData;
        }

        string GetNeighborBorder(WorldBuilder.Neighbors neighbors, WorldBuilder.Utility.Directions dir)
        {
            string neighborBorder = "";
            if(neighbors != null)
            {
                WorldBuilder.Room neighbor = dir == WorldBuilder.Utility.Directions.up ? neighbors.up : dir == WorldBuilder.Utility.Directions.right ? neighbors.right : dir == WorldBuilder.Utility.Directions.down ? neighbors.down : neighbors.left;
                if(neighbor != null)
                {
                    int width = neighbor.map.dimensions.room_width;
                    int height = neighbor.map.dimensions.room_height;
                    if (dir == WorldBuilder.Utility.Directions.up || dir == WorldBuilder.Utility.Directions.down)
                    {
                        height = WorldBuilder.Utility.Directions.up == dir ? 1 : height;
                        for(int i = 1; i <= width; i += 1)
                        {
                            foreach (WorldBuilder.Tile tile in neighbor.map.area)
                            {
                                if (tile.y == height && tile.x == i) neighborBorder += DebugUtility.GetASCIRoomValue( tile.type);
                            }
                        }
                        
                    }
                    else if (dir == WorldBuilder.Utility.Directions.right || dir == WorldBuilder.Utility.Directions.left)
                    {
                        width = WorldBuilder.Utility.Directions.left == dir ? 1 : width;
                        for(int j = 1; j <= height; j += 1)
                        {
                            foreach (WorldBuilder.Tile tile in neighbor.map.area)
                            {
                                if (tile.x == width && tile.y == j) neighborBorder += DebugUtility.GetASCIRoomValue(tile.type);
                            }
                        }
                        
                    }
                }
                
            }
            return neighborBorder;
        }
    }
}
