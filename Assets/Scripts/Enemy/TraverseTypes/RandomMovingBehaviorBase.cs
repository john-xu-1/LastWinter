using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovingBehaviorBase : MonoBehaviour
{
    [SerializeField] float xDistanceBuffer = 0.5f;
    [SerializeField] float movingTimeOut = 5f;
    [SerializeField] float xTranslationMax = 10f, xTranslationMin = 2f;
    [SerializeField] float xDestination;

    float movingTimeOutStart;

    public void Start()
    {
        xDestination = transform.position.x;

    }
    public Vector2 Use(ref Vector2 direction, ref Vector2 velocity)
    {
        
        if (Mathf.Abs(xDestination - transform.position.x) < xDistanceBuffer || movingTimeOutStart + movingTimeOut < Time.time)
        {
            float radius = Random.Range(xTranslationMin, xTranslationMax);
            //flip direction
            if (direction.x > 0) xDestination -= radius;
            else xDestination += radius;

            movingTimeOutStart = Time.time;
        }

        if (transform.position.x > xDestination) direction.x = -1;
        else if (transform.position.x < xDestination) direction.x = 1;
        else direction.x = 0;

        return new Vector2(direction.x * followSpeed(), velocity.y);
    }

    

    private float followSpeed()
    {
        return 2.5f;
    }


}
