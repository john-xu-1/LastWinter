using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace WorldBuilder
{
    [CreateAssetMenu(fileName = "Worlds", menuName = "ASP/Worlds", order = 1)]
    public class Worlds : ScriptableObject
    {
        public string ASPFileName = "Testing";
        [SerializeField]
        public List<Dictionary<string, List<List<string>>>> WorldList = new List<Dictionary<string, List<List<string>>>>();
        public int WorldCount = 0;

        public List<World> BuiltWorlds = new List<World>();


        

       

        //public void AddWorld(Dictionary<string, List<List<string>>> newWorld)
        //{
        //    WorldList.Add(newWorld);
        //    WorldCount = WorldList.Count;
        //}

        public void AddWorld(World world)
        {
            Utility.SaveWorld(world, world.name);
            BuiltWorlds.Add(world);

        }
    }

}
