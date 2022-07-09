using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 target_Offset;
    public CameraFollow followType = CameraFollow.snap;
    public enum CameraFollow { snap, follow, lab}
    private Vector2 origin;

    private Vector2 gridOffset;

    private int worldWidth, worldHeight, roomWidth, roomHeight;

    private Camera camera;

    private Vector2 gridSize;
    private Vector2 resolution;
    //private Vector2 gridSize { get { return new Vector2(camera.orthographicSize * 2, camera.aspect * camera.orthographicSize * 2); } }
    [SerializeField] private float targetOthographicSize = 31;
    [SerializeField] private float targetOthographicSizeGrowthRate = 0.01f;

    [SerializeField] private bool debug = true;
    [SerializeField] private int debugWorldHeight = 100, debugWorldWidth = 100;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //target_Offset = Vector3.forward * -10;
        origin = transform.position;
        camera = GetComponent<Camera>();
        gridSize = FindGridSize();
        resolution = new Vector2(Screen.width, Screen.height);
        targetOthographicSize = camera.orthographicSize;

        if (debug)
        {
            roomHeight = 1;
            roomWidth = 1;
            worldHeight = debugWorldHeight;
            worldWidth = debugWorldWidth;
        }

        //gridOffset = new Vector2(origin.x - gridSize.x / 2, origin.y - gridSize.y / 2);
    }

    Vector2 FindGridSize()
    {
        float height = camera.orthographicSize * 2;
        float width = camera.aspect * height;
        return new Vector2(width, height);
    }
    void FixedUpdate()
    {
        if(!target) target = GameObject.FindGameObjectWithTag("Player").transform;

        if (followType == CameraFollow.snap && target)
        {
            float targetH = target.position.y - (origin.y - gridSize.y / 2);
            float targetW = target.position.x - (origin.x - gridSize.x / 2);
            int gridH = (int)(targetH / gridSize.y);
            int gridW = (int)(targetW / gridSize.x);
            if (targetH < 0) gridH -= 1;
            if (targetW < 0) gridW -= 1;

            transform.position = new Vector3(origin.x + gridW * gridSize.x, origin.y + gridH * gridSize.y, transform.position.z);
        }
        else if(followType == CameraFollow.follow)
        {

            if (target)
            {


                transform.position = target.position + target_Offset;


                //Vector2 cameraOffset = Vector2.zero;
                //if (resolution.x != Screen.width || resolution.y != Screen.height)
                //{
                //    targetOthographicSize = FindMinTargetOthographicSize();
                //    SetCameraSize(targetOthographicSize, true);
                //    resolution = new Vector2(Screen.width, Screen.height);
                //}
                //UpdateOthographicSize();



                //if (target.position.y + gridSize.y / 2 > 0) cameraOffset.y = -target.position.y - gridSize.y / 2;
                //else if (target.position.y - gridSize.y / 2 < -worldHeight * roomHeight) cameraOffset.y = -target.position.y + gridSize.y / 2 - worldHeight * roomHeight;

                //if (target.position.x - gridSize.x / 2 < 1) cameraOffset.x = -target.position.x + gridSize.x / 2 + 1;
                //else if (target.position.x + gridSize.x / 2 > worldWidth * roomWidth + 1) cameraOffset.x = -target.position.x - gridSize.x / 2 + worldWidth * roomWidth + 1;

                //transform.position = target.position + target_Offset + (Vector3)cameraOffset;
            }
        }
        else if(followType == CameraFollow.lab)
        {
            float minX = bound.bounds.min.x + camera.aspect * camera.orthographicSize;
            float minY = bound.bounds.min.y + camera.orthographicSize;
            float maxX = bound.bounds.max.x - camera.aspect * camera.orthographicSize;
            float maxY = bound.bounds.max.y - camera.orthographicSize;

            float x = target.position.x;
            float y = target.position.y;
            float z = transform.position.z;
            if (x < minX) x = minX;
            if (x > maxX) x = maxX;
            if (y < minY) y = minY;
            if (y > maxY) y = maxY;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, z),  followSmoothing * Vector2.Distance(transform.position, target.position) * Time.deltaTime);
        }

    }
    public BoxCollider2D bound;
    public float followSmoothing = 10;

    public void UpdateOthographicSize()
    {
        if (camera.orthographicSize < targetOthographicSize)
        {
            if (targetOthographicSize - camera.orthographicSize < targetOthographicSizeGrowthRate * Time.time)
            {
                setCameraSize (targetOthographicSize);
            }
            else
            {
                camera.orthographicSize += targetOthographicSizeGrowthRate * Time.time;
            }
            UpdateGridSize();
        }
        else if (camera.orthographicSize > targetOthographicSize)
        {
            if (camera.orthographicSize - targetOthographicSize < targetOthographicSizeGrowthRate * Time.time)
            {
                setCameraSize(targetOthographicSize);
            }
            else
            {
                setCameraSize(targetOthographicSize - targetOthographicSizeGrowthRate * Time.time);
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


    private void setCameraSize (float orthagraphicSize)
    {
        camera.orthographicSize = orthagraphicSize;
    }

    public void SetCameraSize(float orthagraphicSize)
    {
        setCameraSize(orthagraphicSize);
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
    public void setUp(bool CameraSnap)
    {
        this.followType = CameraSnap? CameraFollow.snap: CameraFollow.follow;
        setUp(worldWidth, worldHeight, roomWidth, roomHeight, target, CameraSnap);

    }
    public void setUp(int worldWidth, int worldHeight, int roomWidth, int roomHeight, Transform player, bool CameraSnap)
    {
        this.followType = CameraSnap ? CameraFollow.snap : CameraFollow.follow;
        setUp(worldWidth, worldHeight, roomWidth, roomHeight, player);

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
        //Camera camera = GetComponent<Camera>();
        float height = camera.orthographicSize * 2;
        float width = camera.aspect * height;
        gridSize = new Vector2(width, height);

        if (followType == CameraFollow.follow) targetOthographicSize = FindMinTargetOthographicSize();
        else SetCameraSnap();

    }
    int screenHeight = 1080;
    int screenWidth = 1920;

    public void SetCameraSnap()
    {

        if (camera.aspect >= 1)
        {
            SetCameraSize(roomHeight / 2, false);
            targetOthographicSize = roomHeight / 2;
            Screen.SetResolution(screenHeight, screenHeight, true);
        }
        else
        {
            SetCameraSize((roomWidth / 2) / camera.aspect, false);
            targetOthographicSize = (roomWidth / 2) / camera.aspect;
            Screen.SetResolution(screenWidth, screenWidth, true);
        }
        UpdateGridSize(new Vector2(roomWidth, roomHeight));
        origin = new Vector2(1 + roomWidth / 2, -roomHeight / 2);
    }
}

