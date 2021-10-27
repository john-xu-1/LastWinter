using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldBuilder;

namespace Debugging
{
    
    public class DebugUtility : WorldBuilder.SaveUtility
    {
        public static new string DataFilePath = @"Debug/Data";

        public static string GetASCIRoom(WorldBuilder.Room room)
        {

            string asciRoom = "";
            if (room.isDestroyed) return asciRoom;
            int width = room.map.dimensions.room_width;
            int height = room.map.dimensions.room_height;
            List<WorldBuilder.FreeObject> freeObjects = room.items;
            for(int j = 1; j <= height; j += 1)
            {
                for(int i =1; i <= width; i += 1)
                {
                    WorldBuilder.FreeObject item = new FreeObject();
                    foreach (WorldBuilder.FreeObject obj in freeObjects)
                    {
                        if (obj.x == i && obj.y == j) item = obj;
                    }
                    foreach (Tile tile in room.map.area)
                    {
                        
                        if(item.x == i && item.y == j)
                        {
                            if(item.FreeObjectType == FreeObject.FreeObjectTypes.key) asciRoom += "K";
                            else if (item.FreeObjectType == FreeObject.FreeObjectTypes.enemy) asciRoom += "E";
                            else if (item.FreeObjectType == FreeObject.FreeObjectTypes.environment) asciRoom += "V";
                            break;
                        }
                        else if(tile.x == i && tile.y == j)
                        {
                            asciRoom += GetASCIRoomValue(tile.type);
                            //if (tile.type == 0) asciRoom += "-";
                            //else if (tile.type == 1) asciRoom += "X";
                            //else if (tile.type == 2) asciRoom += "W";
                            //else if (tile.type == 3) asciRoom += "L";
                            //else if (tile.type == 4) asciRoom += "D";
                        }
                    }
                }
            }
            return asciRoom;
        }

        public static string GetASCIRoomValue(int index)
        {
            string asciRoom = "";
            if (index == 0) asciRoom += "-";
            else if (index == 1) asciRoom += "X";
            else if (index == 2) asciRoom += "W";
            else if (index == 3) asciRoom += "L";
            else if (index == 4) asciRoom += "D";
            return asciRoom;
        }

        public static T[,] AddRow<T>(T[,] matrix, T[] row)
        {
            int rows = matrix.GetUpperBound(1)+1;
            int cols = matrix.GetUpperBound(0)+1;
            Debug.Log($"{cols} {rows}");
            T[,] newMatrix = new T[cols, rows + 1];
            for(int i = 0; i < rows; i += 1)
            {
                for(int j = 0; j < cols; j += 1)
                {
                    newMatrix[j, i] = matrix[j,i];
                }
            }
            for(int i = 0; i < cols; i += 1)
            {
                newMatrix[i, rows] = row[i];
            }
            return newMatrix;
        }

        public static string ConvertMatrixToString(string [,] matrix)
        {
            string str = "";
            
            for (int j = 0; j < matrix.GetUpperBound(1) + 1; j += 1)
            {

                for (int i = 0; i < matrix.GetUpperBound(0) + 1; i += 1)
                {
                    str += matrix[i, j] + " ";
                }
                str += "\n";
            }
            return str;
        }
    }
}

