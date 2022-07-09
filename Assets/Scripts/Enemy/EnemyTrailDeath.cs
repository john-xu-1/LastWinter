using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrailDeath : MonoBehaviour
{

    public float soft;
    
    void Update()
    {
        if (transform.localScale.z >= 0) transform.localScale -= Vector3.one * Time.deltaTime * soft;
    }
}
