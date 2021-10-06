using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class FreeObject
    {
        public float x, y;
        public FreeObjectTypes FreeObjectType;
        public string Variation { set { SetVariation(value); } get { return GetVariation(); } }
        public string variation;
        public GameObject gameObject;

        public virtual string GetVariation() { throw new System.NotImplementedException(); }
        public virtual void SetVariation(string variation) { throw new System.NotImplementedException(); }
        public virtual void ItemSetup() { throw new System.NotImplementedException(); }
        public virtual void Remove() { throw new System.NotImplementedException(); }



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

        public static Key GetKey(Graph worldGraph, int roomID)
        {
            foreach (Key key in worldGraph.keys)
            {
                if (key.roomID == roomID)
                {
                    return key;
                }
            }
            return null;
        }

        public static string GetKeyRoomRules (Key key, GateTypes[] gates)
        {
            string code = "";
            if (key != null)
            {
                code += GetItemRules(FreeObjectTypes.key, gates[key.type - 1].ToString(), 1, 1);
                code += $@"
                        
                        :- free_object(XX,YY,{FreeObjectTypes.key},_), not path(XX,YY+2,_).
                        :- free_object(XX,YY,{FreeObjectTypes.key},_), not state(XX,YY,zero).

                    ";
            }
            return code;
        }

        public static string GetKeyRoomRules(Graph worldGraph, int roomID, GateTypes[] gates)
        {
            string code = "";

            foreach(Key key in worldGraph.keys)
            {
                if(key.roomID == roomID)
                {
                    //code += GetItemRules(FreeObjectTypes.key, gates[key.type - 1].ToString(), 1, 1);
                    //code += $@"

                    //    :- free_object(XX,YY,{FreeObjectTypes.key},_), not path(XX,YY+2,_).
                    //    :- free_object(XX,YY,{FreeObjectTypes.key},_), not state(XX,YY,zero).

                    //";
                    code += GetKeyRoomRules(key, gates);
                }
            }
            return code;
        }
    }

    
}

