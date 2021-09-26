using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldBuilder
{
    [System.Serializable]
    public class Room
    {
        public Vector2Int pos;
        public Dictionary<string, List<List<string>>> rawMap;
        public Map map;
        //public List<Map> destroyedMaps = new List<Map>();
        public bool isDestroyed;
        public CollisionTile[,] mapGrid;
        public List<CollisionTile> tiles = new List<CollisionTile>();
        public Vector2Int upExit, downExit, rightExit, leftExit;
        public Clingo.ClingoSolver.Status buidStatus;
        public List<List<int>> neighborPermutations;
        public List<int> removedNeighbors;
        public double lastBuildTime;
        public List<FreeObject> items = new List<FreeObject>();
        

        public Room(Vector2Int pos)
        {
            this.pos = pos;
            
            
        }
        public void SetupRoom(Dictionary<string, List<List<string>>> rawMap)
        {
            isDestroyed = false;
            this.rawMap = new Dictionary<string, List<List<string>>>(rawMap);
            map = ConvertMap(this.rawMap);
            items = ConvertItems(rawMap);
            SetupRoom(map);
        }
        public void SetupRoom(Map map)
        {
            this.map = map;
            foreach (Path path in map.pathStarts)
            {
                if (path.type == "top") upExit = new Vector2Int(path.x, path.y);
                if (path.type == "bottom") downExit = new Vector2Int(path.x, path.y);
                if (path.type == "right") rightExit = new Vector2Int(path.x, path.y);
                if (path.type == "left") leftExit = new Vector2Int(path.x, path.y);
            }

            //Debug.Log("mapGrid size: " + map.dimensions.room_width + ", " + map.dimensions.room_height);
            mapGrid = new CollisionTile[map.dimensions.room_width, map.dimensions.room_height];
        }
        public void SetupRoom()
        {
            mapGrid = new CollisionTile[map.dimensions.room_width, map.dimensions.room_height];
        }
        public void BuildRoom(FreeObjects freeObjects)
        {
            float xOffset = map.dimensions.room_width * pos.x;
            float yOffset = map.dimensions.room_height * pos.y;
            foreach(FreeObject freeObject in items)
            {
                freeObject.gameObject = GameObject.Instantiate(freeObjects.GetFreeObjectPrefab(freeObject.FreeObjectType, freeObject.GetVariation()));
                Debug.Log($"-----------{freeObject.FreeObjectType}::{freeObject.variation} at ({freeObject.x},{freeObject.y})---------------");
                freeObject.gameObject.transform.position = new Vector2(0.5f + freeObject.x + xOffset, 0.5f -freeObject.y - yOffset);
            }
        }
        //public void DestroyRoom(int destroyer)
        //{
        //    isDestroyed = true;
        //    destroyedMaps.Add(map);
        //}

        public void DestroyRoom(Tilemap tilemap)
        {
            foreach (CollisionTile tile in tiles)
            {
                UtilityTilemap.DestroyTile(tilemap, (Vector3Int)tile.pos);
            }
            foreach(FreeObject freeObject in items)
            {
                GameObject.Destroy(freeObject.gameObject);
            }
        }

        private List<FreeObject> ConvertItems(Dictionary<string, List<List<string>>> dict)
        {
            List<FreeObject> items = new List<FreeObject>();
            if (dict.ContainsKey("free_object"))
            {
                foreach (List<string> item in dict["free_object"])
                {
                    int x = int.Parse(item[0]);
                    int y = int.Parse(item[1]);
                    //Items.ItemTypes itemType = (Items.ItemTypes)item[2];
                    string variation = item[3];
                    KeyFreeObject key = new KeyFreeObject();
                    key.SetupKey(x, y, (GateTypes)System.Enum.Parse(typeof(GateTypes), variation));
                    items.Add(key);
                }
            }
            
            return items;
        }

        public Map ConvertMap(Dictionary<string, List<List<string>>> dict)
        {
            Map map = new Map();
            map.dimensions = Utility.GetDimensions(dict);
            int width = map.dimensions.room_count_width * map.dimensions.room_width;
            int height = map.dimensions.room_count_height * map.dimensions.room_height;
            //Debug.Log(width + "x" + height);

            map.area = Utility.GetTiles(dict);



            List<Path> paths = new List<Path>();
            List<PathStart> pathStarts = new List<PathStart>();
            foreach (List<string> path in dict["path"])
            {
                if (path.Count == 4 && path[3] == "middle")
                {
                    float x = float.Parse(path[0]);
                    float y = float.Parse(path[1]);
                    map.start = new Vector2(x, y);
                }

                if(path.Count == 4)
                {
                    PathStart pathStart = new PathStart();
                    pathStart.x = int.Parse(path[0]);
                    pathStart.y = int.Parse(path[1]);
                    pathStart.type = path[2];
                    pathStarts.Add(pathStart);
                }

                if (path.Count == 3)
                {
                    Path newpath = new Path();
                    newpath.x = int.Parse(path[0]);
                    newpath.y = int.Parse(path[1]);
                    newpath.type = path[2];
                    paths.Add(newpath);
                }
            }
            map.paths = Utility.GetArray(paths);
            map.pathStarts = Utility.GetArray(pathStarts);

            



            return map;
        }


    }
}