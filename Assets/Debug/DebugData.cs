using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger
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
        public void FinishRuntimeData()
        {
            string table = DebugUtility.CreateCSV(runtimeData);
            Debug.Log(table);
            DebugUtility.CreateFile(table, $"{testDataPath}.csv", DebugUtility.DataFilePath);
        }
    }
}
