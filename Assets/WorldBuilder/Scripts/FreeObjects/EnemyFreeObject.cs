using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class EnemyFreeObject : FreeObject
    {
        public EnemyTypes enemyType;
        public enum EnemyTypes
        {
            baseBoi,
            baseBounce,
            shotgun,
            spike
          
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
            return enemyType.ToString();
        }
        public override void SetVariation(string variation)
        {
            enemyType = (EnemyTypes)System.Enum.Parse(typeof(EnemyTypes), variation);
            this.variation = variation;
        }
    }
}

