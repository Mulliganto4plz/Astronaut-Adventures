using UnityEngine;
using System.Collections;

public class EndLevelScript : MonoBehaviour {

    public GameMaster gameMaster;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            gameMaster.LoadNextLevel();
    }

}
