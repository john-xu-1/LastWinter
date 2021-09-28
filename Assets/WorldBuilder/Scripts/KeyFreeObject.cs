using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class KeyFreeObject : FreeObject
    {

        public WorldBuilder.GateTypes keyType;
        public SpriteRenderer sprite;

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
            sprite.color = color;

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
    }
}

