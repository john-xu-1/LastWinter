using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float Width, Height;
    public Transform CurtainTop, CurtainBottom, CurtainLeft, CurtainRight;
    Vector2 CenterOffset;
    
    void Update()
    {
        //transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);
    }

    public void CameraSetup(float width, float height)
    {
        Width = width;
        Height = height;
        //CenterOffset = centerOffset;
        CenterOffset = new Vector2(width / 2 + 1, -height / 2);
        GetComponent<CameraFollow>().Setup(Target, new Vector2(width, height), CenterOffset);

        CurtainLeft.localPosition = new Vector3(-width / 2, 0, CurtainLeft.localPosition.z);
        CurtainRight.localPosition = new Vector3(width / 2, 0, CurtainRight.localPosition.z);
        CurtainTop.localPosition = new Vector3(0, height/2, CurtainTop.localPosition.z);
        CurtainBottom.localPosition = new Vector3(0, -height/2, CurtainBottom.localPosition.z);

        Camera cam = GetComponent<Camera>();
        float aspectRatio = cam.aspect;
        if(aspectRatio < 1 && width / height >= 1)
        {
            //aspect = cw/ch 
            //aspect = width / newheight 
            //newheight = width / aspect
            //halfHeight = 0.5f * newheight
            float size = 0.5f * width / aspectRatio;
            print("orthographicSize changed to " + size);
            cam.orthographicSize = size;
        }
        else
        {
            
            cam.orthographicSize = height / 2;
        }
        

    }
}
