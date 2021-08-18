using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    Dictionary<string, Color> colors = new Dictionary<string, Color>() { { "top",Color.blue },{ "bottom", Color.red},{ "right",Color.yellow},{"left",Color.green },{"middle",Color.gray } };
    public List<string> nodeTypes = new List<string>();

    public void SetUpPathNode(int x, int y, string type)
    {
        transform.position = new Vector3(x+0.5f, y+0.5f, transform.position.z);
        SetColor(colors[type]);
    }

    public void AddNode(string type)
    {
        nodeTypes.Add(type);
        SetColor(Color.Lerp(GetColor(), colors[type], 0.5f));
    }

    void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    Color GetColor()
    {
        return GetComponent<SpriteRenderer>().color;
    }
}
