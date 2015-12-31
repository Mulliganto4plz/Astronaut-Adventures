using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class ElevatorScript : MonoBehaviour {

    bool animHasPlayed;
    

	
	void Start () {
        
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !animHasPlayed)
        {
            
            animHasPlayed = true;
            Animation anim = gameObject.GetComponent<Animation>();
            anim.Play();
            
            //other.gameObject.GetComponent<PlatformEffector2D>().enabled = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
