using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASPMap
{
    public abstract class Map2D : ASPMap
    {
        protected int width, height;
        public float TileSpacing = 1.1f;

        override public void AdjustCamera()
        {
            Camera cam = Camera.main;
            float aspect = cam.aspect;
            float size = cam.orthographicSize;

            float boardSizeHeight = height * TileSpacing / 2 + (TileSpacing - 1) / 2;
            float boardSizeWidth = width * TileSpacing / 2 + (TileSpacing - 1) / 2;

            float boardAspect = boardSizeWidth / boardSizeHeight;

            float boardSizeX = boardSizeWidth / aspect;
            float boardSize = aspect < boardAspect ? boardSizeX : boardSizeHeight;

            cam.orthographicSize = boardSize;

            float y = height / 2 * (1 + (TileSpacing - 1));
            float x = width / 2 * (1 + (TileSpacing - 1));
            if (width % 2 == 0) x -= (1 + (TileSpacing - 1)) / 2;
            if (height % 2 == 0) y -= (1 + (TileSpacing - 1)) / 2;



            cam.transform.position = new Vector3(x, y, cam.transform.position.z);
        }

        protected void parseMapDimensions(Clingo.AnswerSet answerset, string widthKey, string heightKey)
        {
            foreach (List<string> widths in answerset.Value[widthKey])
            {
                if (int.Parse(widths[0]) > width) width = int.Parse(widths[0]);
            }
            foreach (List<string> h in answerset.Value[heightKey])
            {
                if (int.Parse(h[0]) > height) height = int.Parse(h[0]);
            }
        }
    }
}