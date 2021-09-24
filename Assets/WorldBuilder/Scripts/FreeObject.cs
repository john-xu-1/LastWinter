using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    
    public abstract class FreeObject
    {
        public float x, y;
        public FreeObjectTypes FreeObjectType;
        public string variation { set { SetVariation(value); } get { return GetVariation(); } }
        public GameObject gameObject;

        public abstract string GetVariation();
        public abstract void SetVariation(string variation);
        public abstract void ItemSetup();
        public abstract void Remove();

        

        public enum FreeObjectTypes
        {
            none,
            key, 
            enemy,
            environment
        }

        static string GetItemRules(FreeObjectTypes type, string variation, int min, int max)
        {
            string code = $@"
                #show free_object/4.
                {min}{{free_object(XX,YY,{type},{variation}): width(XX),height(YY)}}{max}.
            ";
            return code;
        }

        public static string GetKeyRoomRules(World world, int roomID, GateTypes[] gates)
        {
            string code = "";

            foreach(Key key in world.worldGraph.keys)
            {
                if(key.roomID == roomID)
                {
                    code += GetItemRules(FreeObjectTypes.key, gates[key.type - 1].ToString(), 1, 1);
                    code += $@"
                        
                        :- free_object(XX,YY,{FreeObjectTypes.key},_), not path(XX,YY+2,_).
                        :- free_object(XX,YY,{FreeObjectTypes.key},_), not state(XX,YY,zero).

                    ";
                }
            }
            return code;
        }
    }

    
}

