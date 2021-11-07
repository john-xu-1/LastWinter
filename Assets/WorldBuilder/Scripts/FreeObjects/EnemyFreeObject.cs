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
            spike,
            boss
          
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

        public void Setup(float x, float y, string variation)
        {
            //this.
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
            while(type == EnemyTypes.boss)
            {
                type = GetEnemyTypeFromPaths(paths);
            }
            string code = GetEnemyRoomRules(type);

            return code;
        }

        public static string GetEnemyRoomRules(EnemyTypes enemyType)
        {
           
            int minCeilingHeight = 10;
            string code = FreeObject.GetItemRules(FreeObject.FreeObjectTypes.enemy, enemyType.ToString(), 1, 1);
            code += $@"
                :- free_object(XX,YY,{FreeObjectTypes.enemy},{enemyType}), not floor(XX,YY+1).

                
                :- free_object(XX,YY,{FreeObjectTypes.enemy},{enemyType}), XX == max_width.
                :- free_object(XX,YY,{FreeObjectTypes.enemy},{enemyType}), XX == 1.

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

