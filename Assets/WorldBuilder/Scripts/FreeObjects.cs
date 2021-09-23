using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class FreeObjects
    {
        public float x, y;
        public enum FreeObjectTypes
        {
            none,
            key,
            enemy,
            environment
        }
        public FreeObjectTypes objectType;
        public string objectVariation;

        public static string GetFreeObjectRules(FreeObjectTypes type, string objectVariation, int min, int max)
        {
            string code = $"{min}{{free_object(XX,YY,{type},{objectVariation}):width(XX),height(YY)}}{max}.";
            return code;
        }

        public static string GetKeyRoomRules(World world, int roomID, GateTypes[] gates)
        {
            string code = "";
            foreach(Key key in world.worldGraph.keys)
            {
                if(key.roomID == roomID)
                {
                    code += GetFreeObjectRules(FreeObjectTypes.key, gates[key.type - 1].ToString(), 1, 1);
                    code += $@"
                        :- free_object(XX,YY,{FreeObjectTypes.key.ToString()},_), not path(XX,YY+headroom,_).
                        :- free_object(XX,YY,{FreeObjectTypes.key.ToString()},_), not state(XX,YY,zero).
                    ";
                }
            }
            return code;
        }
    }


}

