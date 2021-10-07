using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class EnvironmentFreeObject : FreeObject
    {
        public EnvironmentTypes environmentType;
        public enum EnvironmentTypes
        {
            light1,
            light2,
            light3
          
        }
        public override void ItemSetup()
        {
            gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);

        }

        public override void Remove()
        {
            GameObject.Destroy(gameObject);
        }

        public override string GetVariation()
        {
            return environmentType.ToString();
        }
        public override void SetVariation(string variation)
        {
            environmentType = (EnvironmentTypes)System.Enum.Parse(typeof(EnvironmentTypes), variation);
            this.variation = variation;
        }
    }
}

