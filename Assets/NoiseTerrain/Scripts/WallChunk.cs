using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseTerrain
{
    public class WallChunk : NodeChunk
    {
        public WallChunk(RoomChunk roomChunk)
        {
            this.roomChunk = roomChunk;
        }

        public static bool IsValidWall(Vector2Int wallTile, int jumpHeight, bool rightSide, RoomChunk roomChunk)
        {
            int xOffset = rightSide ? 1 : -1;

            // valid wall tils is when there is at least one attached tile that is jumpheight + 1 above the ground

            // valid wall + offset --> count down till zero if not ground (empty) then true else

            // return to org position and go up and count down till zero, if wall exists and surface, true, else false

            int count = jumpHeight + 1;
            int x = wallTile.x;
            int y = wallTile.y;
            if (roomChunk.GetTile(x + xOffset, y))
            {
                return false;
            }

            while (count > 0 && !roomChunk.GetTile(x + xOffset, y))
            {
                count--;
                y++; // down is positive
            }

            if (count == 0) return true;

            y = wallTile.y;
            while (count > 0 && !roomChunk.GetTile(x + xOffset, y) && roomChunk.GetTile(x, y))
            {
                count--;
                y--; // up is negative
            }

            if (count == 0) return true;



            return false;

        }
    }

}
