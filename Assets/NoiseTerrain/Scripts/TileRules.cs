using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseTerrain
{
    [CreateAssetMenu(fileName = "TileRules", menuName = "ScriptableObjects/TileRules")]
    public class TileRules : ScriptableObject
    {
        [System.Serializable]
        public struct TileRule
        {
            public string name { get { return tileSprite.name; } }
            public TileNeighbors.State[] neighbors;

            public UnityEngine.Tilemaps.TileBase tileSprite;
        }

        public TileRule[] tiles;

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
