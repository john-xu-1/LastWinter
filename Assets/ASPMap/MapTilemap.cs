using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ASPMap
{


    public class MapTilemap : Map2D
    {
        string[,] map;
        [SerializeField] Tilemap grid;

        //[SerializeField] MapPixel debugMap;
        //[SerializeField] MapKeyPixel debugMapKeyPixel;
        //public override void AdjustCamera()
        //{

        //}
        int widthOffset, heightOffset;

        public override void DisplayMap(Clingo.AnswerSet answerset, ASPMapKey mapKey)
        {
            widthOffset = 0;
            heightOffset = 0;
            DisplayMap(answerset, mapKey.widthKey, mapKey.heightKey, mapKey.tileKey, mapKey.xIndex, mapKey.yIndex, mapKey.tileTypeIndex, ((MapKeyTileRule)mapKey).dict);
            //debugMap.DisplayMap(answerset, debugMapKeyPixel);
        }

        public /*override*/ void DisplayMap(Clingo.AnswerSet answerset, ASPMapKey mapKey, int widthOffset, int heightOffset)
        {
            this.widthOffset = widthOffset;
            this.heightOffset = heightOffset;
            DisplayMap(answerset, mapKey.widthKey, mapKey.heightKey, mapKey.tileKey, mapKey.xIndex, mapKey.yIndex, mapKey.tileTypeIndex, ((MapKeyTileRule)mapKey).dict);
            //debugMap.DisplayMap(answerset, debugMapKeyPixel);
        }

        public void DisplayMap(Clingo.AnswerSet answerset, string widthKey, string heightKey, string tileKey, int xIndex, int yIndex, int pixelTypeIndex, MapObjectKey<TileBaseTileRules> dict)
        {
            foreach (List<string> widths in answerset.Value[widthKey])
            {
                if (int.Parse(widths[0]) > width) width = int.Parse(widths[0]);
            }
            foreach (List<string> h in answerset.Value[heightKey])
            {
                if (int.Parse(h[0]) > height) height = int.Parse(h[0]);
            }

            minWidth = Mathf.Min(minWidth, widthOffset) + 1;
            minHeight = Mathf.Min(minHeight, heightOffset) + 1;
            maxWidth = Mathf.Max(maxWidth, widthOffset + width);
            maxHeight = Mathf.Max(maxHeight, heightOffset + height);

            map = new string[width, height];
            //map = new string[width, height];

            foreach (List<string> pixelASP in answerset.Value[tileKey])
            {
                int x = int.Parse(pixelASP[xIndex]) - 1;
                int y = int.Parse(pixelASP[yIndex]) - 1;
                if (InvertY) y = height - y - 1;

                string pixelType = pixelASP[pixelTypeIndex];

                map[x, y] = pixelType;// (GravityRoomGenerator.tile_types)System.Enum.Parse(typeof(GravityRoomGenerator.tile_types), pixelType);

            }

            for (int h = 0; h < height; h += 1)
            {
                for (int w = 0; w < width; w += 1)
                {
                    Vector2Int pos = new Vector2Int(w, h);
                    
                    TileBaseTileRules tileRules = dict[map[w, h]];
                    TileBase sprite = tileRules.GetTile(map, pos);

                    //if (InvertY) h = -h;
                    
                    

                    grid.SetTile(new Vector3Int(w + widthOffset, h + heightOffset, 0), sprite);
                    
                }
            }

        }

        
    }
}