using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class KeyFreeObject : FreeObject
    {

        public WorldBuilder.GateTypes keyType;

        public static Dictionary<int, Color> KeyColors = new Dictionary<int, Color>() {
            { 1, Color.blue },
            { 2, Color.red },
            { 3, Color.yellow },
            { 4, Color.cyan },
            { 5, Color.magenta }
        };

        public override void ItemSetup()
        {
            gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);

        }

        public override void Remove()
        {
            GameObject.Destroy(gameObject);
        }

        public void SetupKey(float x, float y, WorldBuilder.GateTypes keyType)
        {

            this.keyType = keyType;
            SetupKeyType(keyType);
            this.x = x;
            this.y = y;
            FreeObjectType = WorldBuilder.FreeObject.FreeObjectTypes.key;
            variation = keyType.ToString();
            //ItemSetup();
        }
        public void SetupKey(float x, float y, WorldBuilder.GateTypes keyType, Color color)
        {

            SetupKey(x, y, keyType);
            //sprite.color = color;

        }

        void SetupKeyType(WorldBuilder.GateTypes keyType)
        {
            //to be overwritten change sprites or key behavior
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) gameObject.SetActive(false);
        }

        public override string GetVariation()
        {
            return keyType.ToString();
        }
        public override void SetVariation(string variation)
        {
            keyType = (GateTypes)System.Enum.Parse(typeof(GateTypes), variation);
            this.variation = variation;
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

        public static string GetKeyRoomRules(Key key, GateTypes[] gates)
        {
            string code = "";
            if (key != null && key.type > 0)
            {
                code += GetItemRules(FreeObjectTypes.key, gates[key.type - 1].ToString(), 1, 1);
                code += $@"
                        
                        :- free_object(XX,YY,{FreeObjectTypes.key},_), not path(XX,YY+2,_).
                        :- free_object(XX,YY,{FreeObjectTypes.key},_), not state(XX,YY,zero).

                    ";
            }

            return code;
        }
    }
}

