using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 target_Offset;
    public bool CameraSnap = true;
    private Vector2 gridSize;
    private Vector2 origin;

    private Vector2 gridOffset;

    private void Start()
    {
        target_Offset = transform.position - target.position;
        origin = transform.position;
        Camera camera = GetComponent<Camera>();
        float height = camera.orthographicSize * 2;
        float width = camera.aspect * height;
        gridSize = new Vector2(width, height);


        //gridOffset = new Vector2(origin.x - gridSize.x / 2, origin.y - gridSize.y / 2);
    }
    void Update()
    {
        if (CameraSnap)
        {
            float targetH = target.position.y - (origin.y - gridSize.y / 2);
            float targetW = target.position.x - (origin.x - gridSize.x / 2);
            int gridH = (int)(targetH / gridSize.y);
            int gridW = (int)(targetW / gridSize.x);
            if (targetH < 0) gridH -= 1;
            if (targetW < 0) gridW -= 1;

            transform.position = new Vector3(origin.x + gridW * gridSize.x, origin.y + gridH * gridSize.y, transform.position.z);
        }
        else
        {
            if (target)
            {
                transform.position = Vector3.Lerp(transform.position, target.position + target_Offset, 0.1f);
            }
        }

    }
}
