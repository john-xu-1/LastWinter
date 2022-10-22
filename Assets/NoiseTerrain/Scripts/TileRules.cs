using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseTerrain
{
    [CreateAssetMenu(fileName = "TileRules", menuName = "ScriptableObjects/ASPTileRules")]
    public class TileRules : ScriptableObject
    {
        [System.Serializable]
        public struct TileRule
        {
            public string name { get { return tileSprite.name; } }
            public TileNeighbors.State[] neighbors;

            public UnityEngine.Tilemaps.TileBase tileSprite;
        }

        public TileRule[] Tiles;

        public virtual UnityEngine.Tilemaps.TileBase GetSprite(bool[] neighbors)
        {
            UnityEngine.Tilemaps.TileBase sprite = null;
            foreach (TileRule tileRule in Tiles)
            {
                if (isMatching(tileRule, neighbors) && !sprite) sprite = tileRule.tileSprite;
                else if (isMatching(tileRule, neighbors)) Debug.LogWarning("Multiple sprites matching.");

                if (sprite) break;
            }

            if (sprite == null) Debug.LogWarning("Tile missing");
            return sprite;
        }

        public virtual bool GetValidTile(bool[] neighbors)
        {
            bool isValid = false;
            foreach (TileRule tileRule in Tiles)
            {
                if (isMatching(tileRule, neighbors) && !isValid) isValid = true;
                else if (isMatching(tileRule, neighbors)) Debug.LogWarning("Multiple sprites matching.");
  
            }

            return isValid;
        }

        bool isMatching(TileRule tile, bool[] neighbors)
        {
            
            bool match = true;
            for (int i = 0; i < 8; i += 1)
            {
                if (tile.neighbors[i] != TileNeighbors.State.none && neighbors[i] != (tile.neighbors[i] == TileNeighbors.State.filled)) match = false;
            }
            return match;
        }
    }

    [System.Serializable]
    public class TileNeighbors
    {
        public enum State
        {
            none,
            filled,
            empty
        }
        public State[] neighbors = new State[8];
    }
}

