using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node : MonoBehaviour
{
    public TextMesh text;
    public SpriteRenderer marker;
    public edge upExit, downExit, rightExit, leftExit;
    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void SetText(int text)
    {
        this.text.text = text.ToString();
    }

    public void SetColor(Color color)
    {
        marker.color = color;
        //if (upExit) upExit.SetColor(color);
        //if (downExit) downExit.SetColor(color);
        //if (rightExit) rightExit.SetColor(color);
        //if (leftExit) leftExit.SetColor(color);
    }
}
