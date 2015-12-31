using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {

    public int moveSpeed = 230;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        Destroy(gameObject, 1.5f);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.isTrigger || other.name == "Player")
            return;
        else
        {
            Destroy(gameObject);
            //Debug.Log(other.name + " Enter");

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.isTrigger || other.name == "Player")
            return;
        else
            Destroy(gameObject);
        //Debug.Log(other.name + " Stay");
    }
}
