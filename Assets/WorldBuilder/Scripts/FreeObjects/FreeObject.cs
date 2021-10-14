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

        public static string GetItemRules(FreeObjectTypes type, string variation, int min, int max)
        {
            string code = $@"
                #show free_object/4.
                {min}{{free_object(XX,YY,{type},{variation}): width(XX),height(YY)}}{max}.
            ";
            return code;
        }

        


    }

    
}

