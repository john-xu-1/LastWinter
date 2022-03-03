using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASPMap
{
    public class MapPixel : Map2D
    {
        public Pixel PixelPrefab;
        private Pixel[,] map;

        

        override public void DisplayMap(Clingo.AnswerSet answerset, ASPMapKey mapKey)
        {
            DisplayMap(answerset, mapKey.widthKey, mapKey.heightKey, mapKey.tileKey, mapKey.xIndex, mapKey.yIndex, mapKey.tileTypeIndex, ((MapKeyPixel)mapKey).colorDict);
        }
        public void DisplayMap(Clingo.AnswerSet answerset, string widthKey, string heightKey, string pixelKey, int xIndex, int yIndex, int pixelTypeIndex, MapObjectKey<Color> colorDict)
        {
            parseMapDimensions(answerset, widthKey, heightKey);

            map = new Pixel[width, height];

            foreach (List<string> pixelASP in answerset.Value[pixelKey])
            {
                int x = int.Parse(pixelASP[xIndex]) - 1;
                int y = int.Parse(pixelASP[yIndex]) - 1;

                string pixelType = pixelASP[pixelTypeIndex];

                Pixel pixel = Instantiate(PixelPrefab, transform).GetComponent<Pixel>();
                pixel.SetPixel(x * TileSpacing, y * TileSpacing, colorDict[pixelType]);
                pixel.AddNote(pixelASP);
            }
        }

        
    }
}

