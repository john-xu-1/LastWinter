using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edge : MonoBehaviour
{
    public SpriteRenderer tip, line;
    public void SetDirection(float angle)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
    }

    public void SetColor(Color color)
    {
        tip.color = color;
        line.color = color;
    }
}
