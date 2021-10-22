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
        public EnemyFreeObject()
        {
            FreeObjectType = FreeObjectTypes.enemy;
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

        public static string GetEnemyRoomRules(List<string> paths)
        {
            EnemyTypes type = GetEnemyTypeFromPaths(paths);
            int minCeilingHeight = 10;
            string code = FreeObject.GetItemRules(FreeObject.FreeObjectTypes.enemy, type.ToString(), 1, 1);
            code += $@"
                :- free_object(XX,YY,{FreeObjectTypes.enemy},{type}), not floor(XX,YY+1).

                
                :- free_object(XX,YY,{FreeObjectTypes.enemy},{type}), XX == max_width.
                :- free_object(XX,YY,{FreeObjectTypes.enemy},{type}), XX == 1.

                headroom_offset(1..{minCeilingHeight}).
            ";

            return code;
        }

        static EnemyTypes GetEnemyTypeFromPaths(List<string> paths)
        {
            string gatedPath = paths[0];
            int size = System.Enum.GetNames(typeof(EnemyTypes)).Length;
            int rand = Random.Range(0, size);
            return (EnemyTypes)rand;
        }
    }
}

