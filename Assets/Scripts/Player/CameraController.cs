using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 target_Offset;
    public bool CameraSnap = true;
    
    private Vector2 origin;

    private Vector2 gridOffset;

    private int worldWidth, worldHeight, roomWidth, roomHeight;

    private Camera camera;

    private Vector2 gridSize;
    private Vector2 resolution;
    //private Vector2 gridSize { get { return new Vector2(camera.orthographicSize * 2, camera.aspect * camera.orthographicSize * 2); } }
    [SerializeField] private float targetOthographicSize = 31;
    [SerializeField] private float targetOthographicSizeGrowthRate = 0.01f;
    private void Start()
    {

        target_Offset = Vector3.forward * -10;
        origin = transform.position;
        camera = GetComponent<Camera>();
        gridSize = FindGridSize();
        resolution = new Vector2(Screen.width, Screen.height);
        targetOthographicSize = camera.orthographicSize;

        //gridOffset = new Vector2(origin.x - gridSize.x / 2, origin.y - gridSize.y / 2);
    }

    Vector2 FindGridSize()
    {
        float height = camera.orthographicSize * 2;
        float width = camera.aspect * height;
        return new Vector2(width, height);
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
                //transform.position = Vector3.Lerp(transform.position, target.position + target_Offset, 0.1f);
                Vector2 cameraOffset = Vector2.zero;
                if (resolution.x != Screen.width || resolution.y != Screen.height)
                {
                    targetOthographicSize = FindMinTargetOthographicSize();
                    SetCameraSize(targetOthographicSize, true);
                    resolution = new Vector2(Screen.width, Screen.height);
                }
                UpdateOthographicSize();
                
                
               
                if (target.position.y + gridSize.y / 2 > 0) cameraOffset.y = -target.position.y - gridSize.y / 2;
                else if (target.position.y - gridSize.y / 2 < -worldHeight*roomHeight) cameraOffset.y = -target.position.y + gridSize.y / 2 - worldHeight * roomHeight;

                if (target.position.x - gridSize.x / 2 < 1) cameraOffset.x = - target.position.x + gridSize.x / 2 + 1;
                else if (target.position.x + gridSize.x / 2 > worldWidth*roomWidth + 1) cameraOffset.x = -target.position.x - gridSize.x / 2 + worldWidth * roomWidth + 1;

                transform.position = target.position + target_Offset + (Vector3)cameraOffset ;
            }
        }
            
    }


    public void UpdateOthographicSize()
    {
        if(camera.orthographicSize < targetOthographicSize)
        {
            if (targetOthographicSize - camera.orthographicSize < targetOthographicSizeGrowthRate * Time.time)
            {
                camera.orthographicSize = targetOthographicSize;
            }
            else
            {
                camera.orthographicSize += targetOthographicSizeGrowthRate;
            }
            UpdateGridSize();
        }
        else if (camera.orthographicSize > targetOthographicSize)
        {
            if (camera.orthographicSize - targetOthographicSize < targetOthographicSizeGrowthRate * Time.time)
            {
                camera.orthographicSize = targetOthographicSize;
            }
            else
            {
                camera.orthographicSize -= targetOthographicSizeGrowthRate;
            }
            UpdateGridSize();
        }
    }

    public void UpdateGridSize()
    {
        gridSize = FindGridSize();
        UpdateGridSize(gridSize);
    }

    public void UpdateGridSize(Vector2 gridSize)
    {
        this.gridSize = gridSize;
    }

    public void SetCameraSize(float orthagraphicSize)
    {
        camera.orthographicSize = orthagraphicSize;
    }
    public void SetCameraSize(float orthagraphicSize, bool updateGridSize)
    {
        SetCameraSize(orthagraphicSize);
        if (updateGridSize) UpdateGridSize();
    }

    public float FindMinTargetOthographicSize()
    {
        if (camera.aspect > 1) return (worldWidth * roomWidth / 2) * (1 / camera.aspect);
        else return (worldWidth * roomWidth / 2);
    }

    public void setUp(int worldWidth, int worldHeight, int roomWidth, int roomHeight, Transform player)
    {
        this.worldHeight = worldHeight;
        this.worldWidth = worldWidth;
        this.roomHeight = roomHeight;
        this.roomWidth = roomWidth;
        target = player;



        target_Offset = Vector3.forward * -10;
        origin = transform.position;
        Camera camera = GetComponent<Camera>();
        float height = camera.orthographicSize * 2;
        float width = camera.aspect * height;
        gridSize = new Vector2(width, height);

        targetOthographicSize = FindMinTargetOthographicSize();
        Debug.Log((worldWidth * roomWidth / 2) * (1 / camera.aspect));
    }
}
