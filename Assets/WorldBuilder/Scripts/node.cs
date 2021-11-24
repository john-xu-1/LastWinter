using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class node : MonoBehaviour
    {
        public TextMesh text;
        public SpriteRenderer marker;
        public edge upExit, downExit, rightExit, leftExit;
        public Sprite gate, key; 
        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void SetText(int text)
        {
            this.text.text = text.ToString();
        }

        public void SetType(string type)
        {
            if (type == "gate") marker.sprite = gate;
            else if (type == "key") marker.sprite = key;
        }

        public void SetColor(Color color)
        {
            marker.color = color;
            //if (upExit) upExit.SetColor(color);
            //if (downExit) downExit.SetColor(color);
            //if (rightExit) rightExit.SetColor(color);
            //if (leftExit) leftExit.SetColor(color);
        }

        public void removeUp()
        {
            upExit.gameObject.SetActive(false);
        }
        public void removeDown()
        {
            downExit.gameObject.SetActive(false);
        }
        public void removeLeft()
        {
            leftExit.gameObject.SetActive(false);
        }
        public void removeRight()
        {
            rightExit.gameObject.SetActive(false);
        }
    }
}