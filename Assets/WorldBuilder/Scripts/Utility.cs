using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public static class Utility
    {
        public static List<T> GetSmallestRandomPermutation<T>(List<List<T>> permutations, bool remove)
        {

            int smallest = int.MaxValue;
            foreach (List<T> permutation in permutations)
            {
                if (smallest > permutation.Count) smallest = permutation.Count;
            }

            List<int> smallestIndices = new List<int>();
            for (int i = 0; i < permutations.Count; i += 1)
            {
                List<T> permutation = permutations[i];
                if (smallest == permutation.Count) smallestIndices.Add(i);
            }
            int rand = Random.Range(0, smallestIndices.Count);
            List<T> smallestPermutation = permutations[smallestIndices[rand]];
            if (remove) permutations.Remove(smallestPermutation);

            return smallestPermutation;
        }
        public static List<List<T>> GetPermutations<T>(List<T> indices)
        {
            return GetPermutations(GetArray(indices));
        }
            public static List<List<T>> GetPermutations<T>(T[] indices)
        {
            List<List<T>> permutations = new List<List<T>>();
            int permutationCount = (int)Mathf.Pow(2, indices.Length);
            for (int countI = 1; countI < permutationCount; countI += 1)
            {
                int count = countI;

                List<T> permutation = new List<T>();
                string binary = "";
                for (int i = 1; i < indices.Length + 1; i += 1)
                {
                    int power = (int)Mathf.Pow(2, indices.Length - i);

                    if (count / power == 1)
                    {
                        permutation.Add(indices[i - 1]);
                    }
                    binary += count / power;
                    count -= power * (count / power);
                }
                permutations.Add(permutation);
                //Debug.Log(binary);
            }
            return permutations;
        }
        public static bool has_opening(Dictionary<string, List<List<string>>> map, string side)
        {
            int height = 0;
            int width = 0;
            foreach (List<string> tile in map["tile"])
            {
                width = Mathf.Max(width, int.Parse(tile[0]));
                height = Mathf.Max(width, int.Parse(tile[1]));
            }
            foreach (List<string> tile in map["state"])
            {
                int x = int.Parse(tile[0]);
                int y = int.Parse(tile[1]);
                if (tile[2] == "zero" && (side == "up" && y == 1 || side == "down" && y == height || side == "left" && x == 1 || side == "right" && x == width)) return true;
            }

            return false;
        }

        public static bool hasOpening(Map map, string side)
        {
            int height = map.dimensions.room_height;
            int width = map.dimensions.room_width;
            foreach(Tile tile in map.area)
            {
                int x = tile.x;
                int y = tile.y;
                if(tile.type == 0 && (side == "up" && y == 1 || side == "down" && y == height || side == "left" && x == 1 || side == "right" && x == width)) return true;
            }
            return false;
        }

        public static Vector2Int roomID_to_index(int roomID, int width, int height)
        {
            roomID -= 1;
            int x = roomID % width;
            int y = roomID / width;
            return new Vector2Int(x, y);
        }

        public static int index_to_roomID(Vector2Int index, int width, int height)
        {
            return index.x + index.y * width + 1;
        }

        public static int Max(List<List<string>> values)
        {
            int max = int.MinValue;
            foreach (List<string> value in values)
            {
                if (int.Parse(value[0]) > max)
                {
                    max = int.Parse(value[0]);
                }
            }
            return max;
        }

        public static Dimensions GetDimensions(Dictionary<string, List<List<string>>> world)
        {
            Dimensions dimensions = new Dimensions();


            if (world.ContainsKey("dimensions"))
            {
                //dimensions.room_count_width = int.Parse(world["dimensions"]["room_count_width"][0]);
            }
            else
            {
                dimensions.room_width = Max(world["width"]);
                dimensions.room_height = Max(world["height"]);
                dimensions.room_count_width = 1;
                dimensions.room_count_height = 1;
            }


            return dimensions;
        }

        public static Tile[] GetTiles(Dictionary<string, List<List<string>>> world)
        {
            int height = Max(world["height"]);
            int width = Max(world["width"]);

            Tile[] area = new Tile[height * width];
            int tileIndex = 0;
            foreach (List<string> tile in world["tile"])
            {
                Tile newTile = new Tile();
                newTile.x = int.Parse(tile[0]);
                newTile.y = int.Parse(tile[1]);
                if (tile[2] == "filled") newTile.type = 1;
                else if (tile[2] == "empty") newTile.type = 0;

                area[tileIndex] = newTile;
                tileIndex += 1;
            }

            return area;
        }

        public static T[] GetArray<T>(List<T> list)
        {
            T[] newList = new T[list.Count];
            for(int i =0; i < list.Count; i += 1)
            {
                newList[i] = list[i];
            }
            return newList;
        }

        //public static T[] GetArray<T>(Dictionary<string, List<List<string>>> dict, string key)
        //{
        //    foreach(List<string> value in dict[key])
        //    {

        //    }
        //}
    }
}