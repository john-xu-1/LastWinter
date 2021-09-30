using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugging
{
    public class DebugData : MonoBehaviour
    {
        [SerializeField] string testDataPath = "GraphCSVTest1";
        string[,] runtimeData;
        public void RuntimeDataStart(int col, int rows)
        {
            runtimeData = new string[col, rows];
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
            for(int i = 0; i < values.Length; i += 1)
            {
                runtimeData[i, runID] = values[i].ToString();
            }
        }
        public void RuntimeData(string[] values, int runID)
        {
            for (int i = 0; i < values.Length; i += 1)
            {
                runtimeData[i, runID] = values[i];
            }
        }
        public void FinishRuntimeData()
        {
            string table = DebugUtility.CreateCSV(runtimeData);
            Debug.Log(table);
            DebugUtility.CreateFile(table, $"{testDataPath}.csv", DebugUtility.DataFilePath);
        }

        public void RoomRuntimeData(int runID, float runtime, DebugRoom debugRoom, WorldBuilder.Room room, Clingo.ClingoSolver.Status buildStatus)
        {
            string[] data = new string[21];
            Vector2Int roomIndex = room.pos;                                data[0] = $"{roomIndex.x} {roomIndex.y}";
            int width = debugRoom.roomSize.x;                               data[1] = width.ToString();
            int height = debugRoom.roomSize.y;                              data[2] = height.ToString();
            int upExit = 0;                                                 data[3] = upExit.ToString();
            int upEntrace = 0;                                              data[4] = upEntrace.ToString();
            int rightExit = 0;                                              data[5] = rightExit.ToString();
            int rightEntrance = 0;                                          data[6] = rightEntrance.ToString();
            int downExit = 0;                                               data[7] = downExit.ToString();
            int downEntrance = 0;                                           data[8] = downEntrance.ToString();
            int leftExit = 0;                                               data[9] = leftExit.ToString();
            int leftEntrance = 0;                                           data[10] = leftEntrance.ToString();
            string gate = GetGateData(debugRoom)[0];                        data[11] = gate;
            string gated = GetGateData(debugRoom)[1];                       data[12] = gated;
            string neighborUp = "";                                         data[13] = neighborUp;
            string neighborRight = "";                                      data[14] = neighborRight;
            string neighborDown = "";                                       data[15] = neighborDown;
            string neighborLeft = "";                                       data[16] = neighborLeft;
            string status = buildStatus.ToString();                         data[17] = status;
            string map = DebugUtility.GetASCIRoom(room);                    data[18] = map;
            int cpus = debugRoom.cpus;                                      data[19] = cpus.ToString();
                                                                            data[20] = runtime.ToString();
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
            if(debugRoom.gate.type != 0)
            {
                gateData[0] = debugRoom.gates[debugRoom.gate.type-1].ToString();
                int source = debugRoom.gate.source;
                int destination = debugRoom.gate.destination;
                if (destination == source - 1) gateData[1] = "left";
                else if (destination == source + 1) gateData[1] = "right";
                else if (destination > source) gateData[1] = "down";
                else if (destination < source) gateData[1] = "up";
            }
            else if (debugRoom.gated.type != 0)
            {
                gateData[0] = debugRoom.gates[debugRoom.gated.type-1].ToString();
            }
            return gateData;
        }
    }
}
