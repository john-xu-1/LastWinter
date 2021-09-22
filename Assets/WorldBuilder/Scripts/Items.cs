using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class Items
    {
        public static Dictionary<int, Color> KeyColors = new Dictionary<int, Color>() {
            { 1, Color.blue },
            { 2, Color.red },
            { 3, Color.yellow },
            { 4, Color.cyan },
            { 5, Color.magenta }
        };

        public enum ItemTypes
        {
            none,
            key
        }

        static string GetItemRules(ItemTypes itemType, string variation, int min, int max)
        {
            string code = $"{min}{{item(XX,YY,{itemType},{variation}): width(XX),height(YY)}}{max}.";
            return code;
        }

        public static string GetKeyRoomRules(World world, int roomID, GateTypes[] gates)
        {
            string code = "";

            foreach(Key key in world.worldGraph.keys)
            {
                if(key.roomID == roomID)
                {
                    code += GetItemRules(ItemTypes.key, gates[key.type - 1].ToString(), 1, 1);
                    code += $@"
                        
                        :- item(XX,YY,key,_), not path(XX,YY+2,_).
                        :- item(XX,YY,key,_), not state(XX,YY,zero).

                    ";
                }
            }
            return code;
        }
    }

    
}

