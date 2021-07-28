using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTriger : MonoBehaviour
{
    public GameObject puzzle;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<PuzzleSpawn>().puz = puzzle;
        }
    }
}
