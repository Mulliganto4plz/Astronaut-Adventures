using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointScript : MonoBehaviour {

    public static Transform latestCheckpoint;
    bool playerReachedMe;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && !playerReachedMe)
        {
            playerReachedMe = true;
            latestCheckpoint = transform;
        }
    }
}
