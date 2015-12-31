using UnityEngine;
using System.Collections;

public class BossFightWallScript : MonoBehaviour {

    GameObject boss;
    bool bossFound = false;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(FindBoss());
	}

    IEnumerator FindBoss()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        if(boss == null)
        {
            yield return new WaitForSeconds(0.5f);
            boss = GameObject.FindGameObjectWithTag("Boss");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (boss != null)
            bossFound = true;
        if(bossFound && boss.GetComponent<MinibossScript>().imDead)
        {
            Destroy(gameObject);
        }
	}
}
