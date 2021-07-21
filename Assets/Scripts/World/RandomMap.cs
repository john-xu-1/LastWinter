using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class RandomMap : MonoBehaviour
    {
        RandomBitmap rbm;
        public MapGenerator Generator;

        [TextArea(5, 10)]
        public string Bitmap;

        private void Start()
        {
            rbm = GetComponent<RandomBitmap>();
            Bitmap = rbm.myString;
            //if (Generator) Generator.ConvertMap(Bitmap);
        }
    }
}