using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldBuilder;

namespace Debugger
{
    
    public class DebugUtility : WorldBuilder.SaveUtility
    {
        public static new string DataFilePath = @"Debug/Data";

        public static string GetASCIRoom(WorldBuilder.Room room)
        {
            string asciRoom = "";
            int width = room.map.dimensions.room_width;
            int height = room.map.dimensions.room_height;
            for(int j = 1; j <= height; j += 1)
            {
                for(int i =1; i <= width; i += 1)
                {
                    foreach(Tile tile in room.map.area)
                    {
                        if(tile.x == i && tile.y == j)
                        {
                            if (tile.type == 0) asciRoom += "-";
                            else if (tile.type == 1) asciRoom += "X";
                            else if (tile.type == 2) asciRoom += "W";
                            else if (tile.type == 3) asciRoom += "L";
                            else if (tile.type == 4) asciRoom += "D";
                        }
                    }
                }
            }
            return asciRoom;
        }
    }
}

