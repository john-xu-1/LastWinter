using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 target_Offset;
    public CameraFollow FollowType = CameraFollow.snap;

    public MenuUIHandler MUH;
    public NoiseTerrain.ProceduralMapGenerator chunkLoader;

    public enum CameraFollow
    {
        snap,
        follow,
        lab,
        menu_selection
    }

    private Vector2 origin;

    private Vector2 gridOffset;

    private int worldWidth, worldHeight, roomWidth, roomHeight;

    private Camera myCamera;

    private List<Color32> colors = new List<Color32>();

    private Vector2 gridSize;
    private Vector2 resolution;
    //private Vector2 gridSize { get { return new Vector2(camera.orthographicSize * 2, camera.aspect * camera.orthographicSize * 2); } }
    [SerializeField] private float targetOthographicSize = 31;
    [SerializeField] private float targetOthographicSizeGrowthRate = 0.01f;

    [SerializeField] private bool debug = true;
    [SerializeField] private int debugWorldHeight = 100, debugWorldWidth = 100;
    int index = 0;

    private float nextInputTime;

    public float inputRate = 0.15f;

    public bool active = false;

    private Vector2Int chunkID;
    private void Start()
    {
        //target_Offset = Vector3.forward * -10;
        origin = transform.position;
        myCamera = GetComponent<Camera>();
        gridSize = FindGridSize();
        resolution = new Vector2(Screen.width, Screen.height);
        targetOthographicSize = myCamera.orthographicSize;

        if (FollowType == CameraFollow.menu_selection)
        {
            for (int i = 0; i < MUH.getArray().Count; i += 1)
            {
                Color32 c = Random.ColorHSV();
                colors.Add(c);
            }

            GetComponent<Camera>().backgroundColor = colors[0];

        }

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
        float height = myCamera.orthographicSize * 2;
        float width = myCamera.aspect * height;
        return new Vector2(width, height);
    }
    void FixedUpdate()
    {
        if (!active) return;
        if (!target && FollowType != CameraFollow.menu_selection) target = GameObject.FindGameObjectWithTag("Player").transform;
        if (FollowType == CameraFollow.snap && target)
        {
            float targetH = target.position.y - (origin.y - gridSize.y / 2);
            float targetW = target.position.x - (origin.x - gridSize.x / 2);
            int gridH = (int)(targetH / gridSize.y);
            int gridW = (int)(targetW / gridSize.x);
            if (targetH < 0) gridH -= 1;
            if (targetW < 0) gridW -= 1;

            transform.position = new Vector3(origin.x + gridW * gridSize.x, origin.y + gridH * gridSize.y, transform.position.z);
        }
        else if (FollowType == CameraFollow.follow)
        {

            if (target)
            {


                transform.position = new Vector3(target.position.x + target_Offset.x, target.position.y + target_Offset.y, target.position.z + target_Offset.z);


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
        else if (FollowType == CameraFollow.lab)
        {
            float minX = bound.bounds.min.x + myCamera.aspect * myCamera.orthographicSize;
            float minY = bound.bounds.min.y + myCamera.orthographicSize;  

            float maxX = bound.bounds.max.x - myCamera.aspect * myCamera.orthographicSize;
            float maxY = bound.bounds.max.y - myCamera.orthographicSize;

            float x = target.position.x;
            float y = target.position.y;
            float z = transform.position.z;
            if (x < minX) x = minX;
            if (x > maxX) x = maxX;
            if (y < minY) y = minY;
            if (y > minY) y = maxY;

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, z), Vector3.Distance(transform.position, target.position) * Time.deltaTime * labSmoothing);
        }
        

    }

    private void Update()
    {
        if (FollowType == CameraFollow.menu_selection)
        {

            if (transform.position.x < MUH.InstantiateInterval * (MUH.getArray().Count - 1))
            {


                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (Time.time >= nextInputTime)
                    {
                        index++;
                        transform.position = new Vector3(transform.position.x + MUH.InstantiateInterval, transform.transform.position.y, transform.position.z);
                        GetComponent<Camera>().backgroundColor = colors[index];
                        //Debug.Log(index - 1 + " + 1");

                        MUH.setSaveName(index);

                        nextInputTime = Time.time + inputRate;
                    }

                }

            }
            if (transform.position.x > 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (Time.time >= nextInputTime)
                    {
                        index--;
                        transform.position = new Vector3(transform.position.x - MUH.InstantiateInterval, transform.transform.position.y, transform.position.z);
                        GetComponent<Camera>().backgroundColor = colors[index];

                        MUH.setSaveName(index);

                        nextInputTime = Time.time + inputRate;
                    }

                }
            }



            //Debug.Log(index);

        }
        else if(chunkLoader)
        {
            Vector2Int chunkID = chunkLoader.GetChunkID(transform.position);
            
            if (this.chunkID == null || chunkID != this.chunkID)
            {
                this.chunkID = chunkID;
                foreach (Vector2Int visibleChunkID in chunkLoader.visibleChunkIDs)
                {
                    int distance = (int)Vector2Int.Distance(visibleChunkID, chunkID);
                    chunkLoader.GetChunk(visibleChunkID).Load(distance);
                }
            }
        }
    }

    public BoxCollider2D bound;
    public float labSmoothing = 2;


    public void UpdateOthographicSize()
    {
        if (myCamera.orthographicSize < targetOthographicSize)
        {
            if (targetOthographicSize - myCamera.orthographicSize < targetOthographicSizeGrowthRate * Time.time)
            {
                setCameraSize (targetOthographicSize);
            }
            else
            {
                myCamera.orthographicSize += targetOthographicSizeGrowthRate * Time.time;
            }
            UpdateGridSize();
        }
        else if (myCamera.orthographicSize > targetOthographicSize)
        {
            if (myCamera.orthographicSize - targetOthographicSize < targetOthographicSizeGrowthRate * Time.time)
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
        myCamera.orthographicSize = orthagraphicSize;
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
        if (myCamera.aspect > 1) return (worldWidth * roomWidth / 2) * (1 / myCamera.aspect);
        else return (worldWidth * roomWidth / 2);
    }
    public void setUp(bool CameraSnap)
    {
        this.FollowType = CameraSnap? CameraFollow.snap : CameraFollow.follow;
        setUp(worldWidth, worldHeight, roomWidth, roomHeight, target, CameraSnap);

    }
    public void setUp(int worldWidth, int worldHeight, int roomWidth, int roomHeight, Transform player, bool CameraSnap)
    {
        this.FollowType = CameraSnap ? CameraFollow.snap : CameraFollow.follow;
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
        float height = myCamera.orthographicSize * 2;
        float width = myCamera.aspect * height;
        gridSize = new Vector2(width, height);

        if (FollowType == CameraFollow.follow) targetOthographicSize = FindMinTargetOthographicSize();
        else SetCameraSnap();

    }
    int screenHeight = 1080;
    int screenWidth = 1920;

    public void SetCameraSnap()
    {

        if (myCamera.aspect >= 1)
        {
            SetCameraSize(roomHeight / 2, false);
            targetOthographicSize = roomHeight / 2;
            Screen.SetResolution(screenHeight, screenHeight, true);
        }
        else
        {
            SetCameraSize((roomWidth / 2) / myCamera.aspect, false);
            targetOthographicSize = (roomWidth / 2) / myCamera.aspect;
            Screen.SetResolution(screenWidth, screenWidth, true);
        }
        UpdateGridSize(new Vector2(roomWidth, roomHeight));
        origin = new Vector2(1 + roomWidth / 2, -roomHeight / 2);
    }
}

