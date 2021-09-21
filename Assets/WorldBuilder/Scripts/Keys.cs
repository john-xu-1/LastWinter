using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class Keys
    {
        public static Dictionary<int, Color> KeyColors = new Dictionary<int, Color>() {
            { 1, Color.blue },
            { 2, Color.red },
            { 3, Color.yellow },
            { 4, Color.cyan },
            { 5, Color.magenta }
        };

        public static string GetKeyRoomRules(World world, int roomID, GateTypes[] gates)
        {
            string code = "";

            foreach(Key key in world.worldGraph.keys)
            {
                if(key.roomID == roomID)
                {
                    code += @$"
                        1{{key(XX,YY,{gates[key.type - 1]}): width(XX), height(YY)}}1 .
                        :- key(XX,YY,_), not path(XX,YY+2,_).
                        :- key(XX,YY,_), not state(XX,YY,zero).

                    ";
                }
            }
            return code;
        }
    }

    
}

